using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chromium;
using Serilog;
using System.Diagnostics;
using TheRobot.Requests;
using TheRobot.Response;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace TheRobot
{
    public class Robot : IRobot, IDisposable
    {
        private IWebDriver _driver { get; set; }
        private IHttpClientFactory _httpClientFactory { get; set; }

        public string DownloadFolder { get; private set; }

        public Robot(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            var Processes = Process.GetProcesses();

            Processes.Where(p => p.ProcessName.ToLower().Contains("chrome")).ToList().ForEach(x => x.Kill());

            DownloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "RobotDownloads");
            if (!Directory.Exists(DownloadFolder))
            {
                Directory.CreateDirectory(DownloadFolder);
            }
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/therobot.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()

                .CreateLogger();

            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            ChromeOptions options = new();

            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.default_directory", DownloadFolder);
            options.AddArgument("--log-level=OFF");
            options.AddExcludedArgument("enable-logging");

            Log.Logger.Information("Starting the selenium driver");

            _driver = new ChromeDriver(options);

            _driver.Manage().Window.Maximize();
            Log.Logger.Information("Selenium driver started");
        }

        public void Dispose()
        {
            _driver?.Quit();
        }

        public async Task<RobotResponse> Execute(IRobotRequest request)
        {
            if (_driver == null)
            {
                throw new Exception("Driver not loaded!");
            }

            if (request.Timeout == null)
            {
                request.Timeout = TimeSpan.FromSeconds(5);
            }

            Log.Information("About to execute {@IRoboRequest}", request);
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
                    Log.Information("The request was not successfully");
                }

                if (request.DelayAfter.Ticks > 0)
                {
                    await Task.Delay(request.DelayAfter);
                }

                request.PostExecute?.Invoke(_driver);
            }
            catch (Exception ex) when (ExecuteExceptionFilter(ex))
            {
                Log.Information("An exception was thrown in the request execution.\nThe exception: {@Exception}", ex);
                response.Status = RobotResponseStatus.ExceptionOccurred;
                response.ErrorMessage = ex.Message;
            }
            catch (WebDriverException ex)
            {
                Log.Error("An critical exception occurs at the robot driver.\nThe exception: {@Exception}", ex);
                response.Status = RobotResponseStatus.ExceptionOccurred;
                response.ErrorMessage = ex.Message;
            }

            Log.Information("{@IRoboRequest} Executed", request);
            return response;
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