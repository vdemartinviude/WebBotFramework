using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chromium;
using System.Diagnostics;
using TheRobot.Requests;
using TheRobot.Response;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using Microsoft.Extensions.Logging;
using TheRobot.WebBotInterfaces;
using TheRobot.WebRequestsParameters;

namespace TheRobot
{
    public class Robot : IRobot, IDisposable
    {
        private IWebDriver? _driver { get; set; }
        private IHttpClientFactory _httpClientFactory { get; set; }
        private readonly ILogger<Robot> _logger;

        public string DownloadFolder { get; private set; }

        public Robot(IHttpClientFactory httpClientFactory, ILogger<Robot> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            ChromeOptions options = new();

            options.AddUserProfilePreference("download.prompt_for_download", false);
            //options.AddUserProfilePreference("download.default_directory", DownloadFolder);
            options.AddArgument("--log-level=OFF");
            options.AddExcludedArgument("enable-logging");

            logger!.LogInformation("Starting the selenium driver");

            _driver = new ChromeDriver(options);

            _driver.Manage().Window.Maximize();
            logger!.LogInformation("Selenium driver started");
        }

        public void Dispose()
        {
            _driver?.Quit();
        }

        public async Task<RobotResponse> Execute(IRobotRequest request)
        {
            request.logger = _logger;

            if (request.Timeout == null)
            {
                request.Timeout = TimeSpan.FromSeconds(5);
            }

            _logger.LogInformation("About to execute {@IRoboRequest}", request);
            RobotResponse response = new();

            try
            {
                if (request.Timeout == null)
                {
                    request.Timeout = TimeSpan.FromSeconds(5);
                }

                request.PreExecute?.Invoke(_driver);

                if (request.DelayBefore.Ticks > 0)
                {
                    await Task.Delay(request.DelayBefore);
                }

                response = request.Exec(_driver);

                if (response.Status != RobotResponseStatus.ActionRealizedOk)
                {
                    _logger.LogInformation("The request was not successfully");
                }

                if (request.DelayAfter.Ticks > 0)
                {
                    await Task.Delay(request.DelayAfter);
                }

                request.PostExecute?.Invoke(_driver);
            }
            catch (Exception ex) when (ExecuteExceptionFilter(ex))
            {
                _logger.LogInformation("An exception was thrown in the request execution.\nThe exception: {@Exception}", ex);
                response.Status = RobotResponseStatus.ExceptionOccurred;
                response.ErrorMessage = ex.Message;
            }
            catch (WebDriverException ex)
            {
                _logger.LogError("An critical exception occurs at the robot driver.\nThe exception: {@Exception}", ex);
                response.Status = RobotResponseStatus.ExceptionOccurred;
                response.ErrorMessage = ex.Message;
            }

            _logger.LogInformation("{@IRoboRequest} Executed", request);
            return response;
        }

        public void Exec2(IWebBotRequest request, IWebBotRequestParameter requestParameter)
        {
            request.ExecRequest(_driver!, requestParameter);
        }

        private bool ExecuteExceptionFilter(Exception ex)
        {
            return ex is NoSuchElementException ||
                   ex is WebDriverTimeoutException ||
                   ex is NoSuchFrameException ||
                   ex is NoSuchWindowException;
        }
    }
}