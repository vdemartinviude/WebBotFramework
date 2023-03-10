using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V108.Runtime;
using RobotTests.Fixtures;
using TheRobot;
using TheRobot.MediatedRequests;

namespace RobotTests;

public class SimpleRobotTests : IClassFixture<RobotFixtures>
{
    private readonly RobotFixtures robotFixtures;

    public SimpleRobotTests(RobotFixtures robotFixtures)
    {
        this.robotFixtures = robotFixtures;
    }

    [Fact]
    public void AssureThatFixturesCanBeCreated()
    {
        Assert.NotNull(robotFixtures);
    }

    [Fact]
    public async Task AssureThatRobotCanNavigateAsync()
    {
        var token = robotFixtures.TokenSource.Token;
        var result = await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = "http://www.uol.com.br"
        }, token);

        Assert.Multiple(() => Assert.True(result.IsT1),
                        () => Assert.Equal("https://www.uol.com.br/", result.AsT1.CurrentUrl),
                        () => Assert.Equal("UOL - Seu universo online", result.AsT1.CurrentPageTitle));
    }

    [Fact]
    public async Task AssureThatRobotCanClick()
    {
        var token = robotFixtures.TokenSource.Token;
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = "https://www.rpachallenge.com/"
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedClickRequest
        {
            BaseParameters = new() { By = By.XPath("//input[@type='submit']") },
            Kind = KindOfClik.ClickByDriver
        }, token);

        Assert.Multiple(() => Assert.True(result.IsT1),
                        () => Assert.Equal("input", result.AsT1.WebElement.TagName));
    }

    [Fact]
    public async Task AssureThatRobotCanClickByJS()
    {
        var token = robotFixtures.TokenSource.Token;
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = "https://www.google.com.br/"
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedClickRequest
        {
            BaseParameters = new() { By = By.XPath("//a[text()='Sobre']") },
            Kind = KindOfClik.ClickByJavaScriptWithFocus
        }, token);
        Assert.Multiple(() => Assert.True(result.IsT1),
                        () => Assert.Equal("Google - Sobre", result!.AsT1!.CurrentPageTitle!));
    }

    [Fact]
    public async Task AssureThatRobotCanAccessAnIframe()
    {
        var token = robotFixtures.TokenSource.Token;
        var pageFilePath = Path.GetFullPath(@"WebPagesForTests\IFrameTest.html");
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = "file://" + pageFilePath,
        }, token);

        await robotFixtures.Robot.Execute(new MediatedChangeFrameRequest()
        {
            BaseParameters = new() { By = By.Id("myiframe") }
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedClickRequest
        {
            BaseParameters = new() { By = By.Id("buttononiframe") },
            Kind = KindOfClik.ClickByDriver
        }, token);

        Assert.True(result.IsT1);
    }

    [Fact]
    public async Task AssureThatRobotCanSetTextByDriver()
    {
        var token = robotFixtures.TokenSource.Token;
        var pageFilePath = Path.GetFullPath(@"WebPagesForTests\IFrameTest.html");
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = pageFilePath,
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedSetTextRequest
        {
            KindOfSetText = KindOfSetText.SetByWebDriver,
            BaseParameters = new() { By = By.Id("textField"), TimeOut = TimeSpan.FromSeconds(5) },
            TextToSet = "Vinicius"
        }, token);

        Assert.True(result.IsT1);
    }

    [Fact]
    public async Task AssureRobotCanSetTextByKeyDown()
    {
        var token = robotFixtures.TokenSource.Token;
        var pageFilePath = Path.GetFullPath(@"WebPagesForTests\IFrameTest.html");
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = pageFilePath,
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedSetTextRequest
        {
            KindOfSetText = KindOfSetText.SetWithKeyPress,
            BaseParameters = new() { By = By.Id("textField"), TimeOut = TimeSpan.FromSeconds(5) },
            TextToSet = "Vinicius"
        }, token);

        Assert.True(result.IsT1);
    }

    [Fact]
    public async Task AssureRobotCanSetTextByKeyDownAnBackSpace()
    {
        var token = robotFixtures.TokenSource.Token;
        var pageFilePath = Path.GetFullPath(@"WebPagesForTests\IFrameTest.html");
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = pageFilePath,
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedSetTextRequest
        {
            KindOfSetText = KindOfSetText.SetWithBackSpaceAndKeyPress,
            BaseParameters = new() { By = By.Id("textFieldWithValue"), TimeOut = TimeSpan.FromSeconds(5) },
            TextToSet = "Vinicius",
            numberOfBackSpaces = 20
        }, token);

        Assert.True(result.IsT1);
    }

    [Fact]
    public async Task AssureRobotCanSetTextByJs()
    {
        var token = robotFixtures.TokenSource.Token;
        var pageFilePath = Path.GetFullPath(@"WebPagesForTests\IFrameTest.html");
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = pageFilePath,
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedSetTextRequest
        {
            KindOfSetText = KindOfSetText.SetWithJs,
            BaseParameters = new() { By = By.Id("textFieldWithValue"), TimeOut = TimeSpan.FromSeconds(5) },
            TextToSet = "Vinicius",
        }, token);

        Assert.True(result.IsT1);
    }
}