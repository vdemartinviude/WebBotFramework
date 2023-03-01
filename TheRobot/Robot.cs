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

namespace TheRobot
{
    public class Robot : IRobot, IDisposable
    {
        private IWebDriver _driver { get; set; }
        private IHttpClientFactory _httpClientFactory { get; set; }
        private readonly ILogger<Robot> _logger;

        public string DownloadFolder { get; private set; }

        public Robot(IHttpClientFactory httpClientFactory, ILogger<Robot> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
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

        private bool ExecuteExceptionFilter(Exception ex)
        {
            return ex is NoSuchElementException ||
                   ex is WebDriverTimeoutException ||
                   ex is NoSuchFrameException ||
                   ex is NoSuchWindowException;
        }
    }
}