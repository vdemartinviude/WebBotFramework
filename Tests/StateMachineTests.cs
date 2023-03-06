using RobotTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}