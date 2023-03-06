using JsonDocumentsManager;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;
using TheRobot;
using TheRobot.DriverService;
using TheRobot.PipelineExceptionHandler;
using TheStateMachine;
using TheStateMachine.Helpers;

namespace RobotTests.Fixtures;

public class RobotAndMachineFixtures : IDisposable
{
    public readonly Robot Robot;
    public readonly ILogger<Robot> Logger;
    public readonly CancellationTokenSource TokenSource;
    public readonly TheMachine StateMachine;
    public readonly InputJsonDocument InputJsonDocument;

    public RobotAndMachineFixtures()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/robtotest.log")
            .CreateLogger();

        IHost host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(x => x.AddJsonFile(@"Configuration\RobotConfiguration.json"))
            .ConfigureServices(services =>
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("TheRobot")));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorPipelineBehavior<,>));
                services.AddSingleton<WebDriverService>();
                services.AddSingleton(x => new InputJsonDocument("InputDataForTests\\InputJson.json"));
            })
            .UseSerilog()
            .Build();
        var thelogger = host.Services.GetRequiredService<ILogger<Robot>>();
        Robot = new Robot(
            host.Services.GetRequiredService<IMediator>(),
            host.Services.GetRequiredService<WebDriverService>(),
            host.Services.GetRequiredService<IConfiguration>());

        StateMachine = new TheMachine(Robot, host.Services.GetRequiredService<InputJsonDocument>(), null, host.Services.GetRequiredService<IConfiguration>(), host.Services.GetRequiredService<ILogger<TheMachine>>(),
            TheStateMachineHelpers.GetMachineSpecification(Assembly.Load("StatesForTests")));
        TokenSource = new();
        InputJsonDocument = host.Services.GetRequiredService<InputJsonDocument>();
    }

    public void Dispose()
    {
        Robot.Dispose();
    }
}