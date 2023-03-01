using RobotTests.Fixtures;
using TheRobot.Requests;
using TheRobot.WebBotRequests;
using TheRobot.WebRequestsParameters;

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
    public void AssureRobotCanNavigate()
    {
        robotFixtures.Robot.Exec2(new WBR_NavigateRequest(), new NavigateRequestParameters
        {
            Url = "http://www.uol.com.br"
        });
    }
}