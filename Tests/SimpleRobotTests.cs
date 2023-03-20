using OpenQA.Selenium;
using RobotTests.Fixtures;
using TheRobot.MediatedRequests;

namespace RobotTests;

public class SimpleRobotTests : IClassFixture<RobotAndMachineFixtures>
{
    private readonly RobotAndMachineFixtures robotFixtures;

    public SimpleRobotTests(RobotAndMachineFixtures robotFixtures)
    {
        this.robotFixtures = robotFixtures;
    }

    [Fact]
    public void AssureThatFixturesCanBeCreated()
    {
        Assert.NotNull(robotFixtures);
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
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.XPath("//input[@type='submit']")) },
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
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.XPath("//a[text()='Sobre']")) },
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
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("myiframe")) }
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedClickRequest
        {
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("buttononiframe")) },
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
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("textField")), TimeOut = TimeSpan.FromSeconds(5) },
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
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("textField")), TimeOut = TimeSpan.FromSeconds(5) },
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
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("textFieldWithValue")), TimeOut = TimeSpan.FromSeconds(5) },
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
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("textFieldWithValue")), TimeOut = TimeSpan.FromSeconds(5) },
            TextToSet = "Vinicius",
        }, token);

        Assert.True(result.IsT1);
    }

    [Theory]
    [InlineData(KindOfSelect.SelectByDriveByText, "Zezinho", "")]
    [InlineData(KindOfSelect.SelectByDriveByValue, "", "1")]
    public async Task AssureRobotCanSelect(KindOfSelect kindOfSelect, string text, string value)
    {
        var token = robotFixtures.TokenSource.Token;
        var pageFilePath = Path.GetFullPath(@"WebPagesForTests\IFrameTest.html");
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = pageFilePath,
        }, token);

        var result = await robotFixtures.Robot.Execute(new MediatedSelectRequest
        {
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("selecttest")) },
            KindOfSelect = kindOfSelect,
            Text = text,
            Value = value
        }, token);

        Assert.True(result.IsT1);
    }

    [Fact]
    public async Task AssureRobotCanUpoloadFile()
    {
        var token = robotFixtures.TokenSource.Token;
        var pageFilePath = Path.GetFullPath(@"WebPagesForTests\IFrameTest.html");
        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            Url = pageFilePath,
        }, token);

        var res = await robotFixtures.Robot.Execute(new MediatedUploadFileBySelectRequest
        {
            BaseParameters = new() { ByOrElement = new(By.Id("myFile")) },
            FilePath = Path.GetFullPath(@"WebPagesForTests\Assets\Image.png")
        }, token);

        Assert.True(res.IsT1);
    }

    [Fact]
    public async Task AssureRobotCanClickShadowDom()
    {
        var token = robotFixtures.TokenSource.Token;

        await robotFixtures.Robot.Execute(new MediatedNavigationRequest
        {
            BaseParameters = new() { DelayAfter = TimeSpan.FromSeconds(10) },
            Url = "https://portalhome.eneldistribuicaosp.com.br/#/autenticacao/login"
        }, token);

        await robotFixtures.Robot.Execute(new MediatedClickRequest
        {
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.Id("truste-consent-button")), DelayAfter = TimeSpan.FromSeconds(20) }
        }, token);

        var shadow = await robotFixtures.Robot.Execute(new MediatedGetShadowRoot
        {
            BaseParameters = new() { ByOrElement = new GenericWebElement(By.XPath("//enel-login-welcome")), DelayAfter = TimeSpan.FromSeconds(5) }
        }, token);

        await robotFixtures.Robot.Execute(new MediatedClickRequest
        {
            BaseParameters = new()
            {
                ByOrElement = new GenericWebElement(new ElementFromSearchContext { SearchContext = shadow.AsT1.SearchContext, CssSelector = "enel-button" })
            }
        }, token);
        Assert.True(shadow.IsT1);
    }
}