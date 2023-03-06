﻿using RobotTests.Fixtures;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheStateMachine.Helpers;

namespace RobotTests;

public class StateMachineTests : IClassFixture<RobotAndMachineFixtures>
{
    private readonly RobotAndMachineFixtures _Robotfixture;

    public StateMachineTests(RobotAndMachineFixtures robotfixture)
    {
        _Robotfixture = robotfixture;
    }

    [Fact]
    public void AssureMachineCanBuild()
    {
        _Robotfixture.StateMachine.Build();
    }

    [Fact]
    public void AssureAllStatesReadFromAssembly()
    {
        var machinespec = TheStateMachineHelpers.GetMachineSpecification(Assembly.LoadFrom("StatesForTests"));
        var states = machinespec.States!.OrderBy(x => x.Name).ToList();

        Assert.Collection(states, x => Assert.Equal("FirstTestState", x.Name),
                                  x => Assert.Equal("SecondTestState", x.Name));
    }

    [Fact]
    public async void AssureCanExecuteFirstState()
    {
        var machinespec = TheStateMachineHelpers.GetMachineSpecification(Assembly.LoadFrom("StatesForTests"));
        var states = machinespec.States!.OrderBy(x => x.Name).ToList();

        await ((BaseState)Activator.CreateInstance(states.Single(x => x.Name == "FirstTestState"), new object[] { _Robotfixture.Robot, null, null })!).Execute(_Robotfixture.TokenSource.Token);
        Assert.True(true);
    }

    [Fact]
    public async void AssureCanExecuteSecondState()
    {
        var machinespec = TheStateMachineHelpers.GetMachineSpecification(Assembly.LoadFrom("StatesForTests"));
        var states = machinespec.States!.OrderBy(x => x.Name).ToList();

        await ((BaseState)Activator.CreateInstance(states.Single(x => x.Name == "FirstTestState"), new object[] { _Robotfixture.Robot, null, null })!).Execute(_Robotfixture.TokenSource.Token);
        await ((BaseState)Activator.CreateInstance(states.Single(x => x.Name == "SecondTestState"), new object[] { _Robotfixture.Robot, _Robotfixture.InputJsonDocument, null })!).Execute(_Robotfixture.TokenSource.Token);
    }
}