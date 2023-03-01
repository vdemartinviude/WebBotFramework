using RobotTests.Fixtures;

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
}