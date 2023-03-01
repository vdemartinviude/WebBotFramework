using RobotTests.Fixtures;
using TheRobot.Requests;

namespace RobotTests;

public class SimpleRobotTests : IClassFixture<RobotFixtures>
{
    private RobotFixtures robotFixtures;

    public SimpleRobotTests(RobotFixtures robotFixtures)
    {
        this.robotFixtures = robotFixtures;
    }

    [Fact]
    public void Test1()
    {
        Assert.NotNull(robotFixtures);
    }

    [Fact]
    public async Task EnsureRobotCanStartAsync()
    {
        await robotFixtures.Robot.Execute(new InitializeBrowserRequest());
    }
}