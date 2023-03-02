using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;
using Microsoft.Extensions.Logging;

namespace TheRobot.DriverService;

public class WebDriverService : IDisposable
{
    private readonly ILogger<WebDriverService> _logger;
    private readonly WebDriver WebDriver;

    public string DownloadFolder { get; private set; }

    public WebDriver GetWebDriver() => WebDriver;

    public WebDriverService(ILogger<WebDriverService> logger)
    {
        new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
        DownloadFolder = Path.GetTempPath();

        ChromeOptions options = new();

        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.default_directory", DownloadFolder);
        options.AddArgument("--log-level=OFF");
        options.AddExcludedArgument("enable-logging");

        logger!.LogInformation("Starting the selenium driver");

        WebDriver = new ChromeDriver(options);

        WebDriver.Manage().Window.Maximize();
        _logger = logger;
    }

    public void Dispose()
    {
        _logger.LogCritical("Encerrando driver!");
        WebDriver.Quit();
    }
}