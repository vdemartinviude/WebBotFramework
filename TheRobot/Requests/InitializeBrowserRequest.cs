using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Response;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;
using Microsoft.Extensions.Logging;

namespace TheRobot.Requests;

public class InitializeBrowserRequest : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        //DownloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "RobotDownloads");
        //if (!Directory.Exists(DownloadFolder))
        //{
        //    Directory.CreateDirectory(DownloadFolder);
        //}

        new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        ChromeOptions options = new();

        options.AddUserProfilePreference("download.prompt_for_download", false);
        //options.AddUserProfilePreference("download.default_directory", DownloadFolder);
        options.AddArgument("--log-level=OFF");
        options.AddExcludedArgument("enable-logging");

        logger!.LogInformation("Starting the selenium driver");

        driver = new ChromeDriver(options);

        driver.Manage().Window.Maximize();
        logger!.LogInformation("Selenium driver started");

        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}