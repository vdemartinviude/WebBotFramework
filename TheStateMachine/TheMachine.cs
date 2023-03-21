using Appccelerate.StateMachine;
using Appccelerate.StateMachine.AsyncMachine;
using JsonDocumentsManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StatesAndEvents;
using System.Reflection;
using TheRobot;
using TheStateMachine.Model;

namespace TheStateMachine
{
    public class TheMachine
    {
        private readonly IConfiguration _configuration;
        private readonly InputJsonDocument _inputDocument;
        private readonly ILogger<TheMachine> _logger;

        private readonly MachineSpecification _machineSpecification;
        private readonly ResultJsonDocument _resultDocument;
        private readonly ILoggerFactory _loggerFactory;

        private readonly Robot _robot;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken token;

        public TheMachine(Robot robot, InputJsonDocument inputJsonDocument, ResultJsonDocument resultJsonDocument, IConfiguration configuration, ILogger<TheMachine> logger, MachineSpecification machineSpecification, ILoggerFactory loggerFactory)
        {
            _robot = robot;
            _inputDocument = inputJsonDocument;
            _resultDocument = resultJsonDocument;
            _configuration = configuration;
            _logger = logger;

            cts = new();
            token = cts.Token;
            _machineSpecification = machineSpecification;
            _loggerFactory = loggerFactory;
        }

        public AsyncActiveStateMachine<BaseState, MachineEvents>? Machine { get; private set; }
        public bool RobotWorking { get; private set; }

        public void Build()
        {
            StateMachineDefinitionBuilder<BaseState, MachineEvents> builder = new();
            BaseState? theFirstState = null;

            var states = _machineSpecification.States!
                .Select(st => (BaseState)Activator.CreateInstance(st, new object[] {
                new StateInfrastructure {
                    LoggerFactory = _loggerFactory,
                    Robot = _robot,
                    InputJsonDocument = _inputDocument,
                    ResultJsonDocument = _resultDocument}})!).ToList();
            foreach (var state in states)
            {
                builder.In(state).ExecuteOnEntry(async () => await MachineExecuteState(state));
                builder.In(state).On(MachineEvents.FinalizeMachine).Execute(() => _endMachine());
                IntermediaryGuardsProcess(builder, states, state);
                FinalGuardProcess(builder, state);
                builder.In(state).On(MachineEvents.NormalTransition).Execute(() => _endMachineByNoGuardActivate());
                if (!_machineSpecification!.IntermediaryGuards!.Any(g => g.NextState!.Name == state.GetType().Name))
                    theFirstState = state;
            }
            builder.WithInitialState(theFirstState!);
            Machine = builder.Build().CreateActiveStateMachine();
        }

        public void ExecuteMachine()
        {
            RobotWorking = true;
            Machine!.Start();
        }

        public async Task ExecuteSingleState(string stateName)
        {
            var state = (BaseState)Activator.CreateInstance(_machineSpecification.States.Single(s => s.Name == stateName), new object[] {
                new StateInfrastructure {
                    LoggerFactory = _loggerFactory,
                    Robot = _robot,
                    InputJsonDocument = _inputDocument,
                    ResultJsonDocument = _resultDocument}})!;
            await state.Execute(token);
        }

        private void _endMachineByNoGuardActivate()
        {
            _logger.LogCritical("No guard was fired for the current state!");
            _endMachine();
        }

        private void _endMachine()
        {
            _robot.Dispose();
            Machine!.Stop();
            RobotWorking = false;
        }

        private void _normalFinish()
        {
            _robot.Dispose();
            Machine!.Stop();
            RobotWorking = false;
        }

        private void FinalGuardProcess(StateMachineDefinitionBuilder<BaseState, MachineEvents> builder, BaseState? currentState)
        {
            var finalGuardsForState = _machineSpecification.FinalGuards!.Where(x => x.CurrentState!.Name == currentState!.GetType().Name)
                                .Select(x => new
                                {
                                    x.CurrentState,
                                    TheGuard = Activator.CreateInstance(x.Guard!, Array.Empty<object>()),
                                    x.Guard
                                });
            foreach (var guard in finalGuardsForState)
            {
                builder
                    .In(currentState!)
                    .On(MachineEvents.NormalTransition)
                    .If(async () => await ((Task<bool>)guard.Guard!.GetMethod("Condition")!.Invoke(guard.TheGuard, new object[] { _robot, token })!))
                    .Execute(() => _normalFinish());
            }
        }

        private void IntermediaryGuardsProcess(StateMachineDefinitionBuilder<BaseState, MachineEvents> builder, List<BaseState> states, BaseState? currentState)
        {
            var interGuardsForState = _machineSpecification.IntermediaryGuards!.Where(x => x.CurrentState!.Name == currentState!.GetType().Name).
                                Select(x => new
                                {
                                    x.CurrentState,
                                    x.NextState,
                                    TheGuard = Activator.CreateInstance(x.Guard!, Array.Empty<object>()),
                                    x.Guard
                                })
                                .OrderBy(x => (uint)x.Guard!.GetProperty("Priority")!.GetValue(x.TheGuard)!);
            foreach (var guard in interGuardsForState)
            {
                var nextstate = states.Where(x => x.GetType().Name == guard!.NextState!.Name).Single();
                builder
                    .In(currentState!)
                    .On(MachineEvents.NormalTransition)
                    .If(async () => await ((Task<bool>)guard.Guard!.GetMethod("Condition")!.Invoke(guard.TheGuard, new object[] { _robot, token })!))
                    .Goto(nextstate);
            }
        }

        private async Task MachineExecuteState(BaseState state)
        {
            Task[] tasks = { state.Execute(token) };
            _logger.LogWarning($"Executing the state: {state.Name}");

            var completed = Task.WaitAll(tasks, (int)state.StateTimeout.TotalMilliseconds, token);
            if (completed)
            {
                await Machine!.Fire(MachineEvents.NormalTransition);
            }
            else
            {
                _logger.LogCritical("State {name} timeout", state!.GetType().Name);
                cts.Cancel();
                await Machine!.FirePriority(MachineEvents.FinalizeMachine);
            }
        }
    }
}