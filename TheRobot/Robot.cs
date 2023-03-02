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
using TheRobot.WebRequestsParameters;
using MediatR;
using TheRobot.MediatedRequests;
using TheRobot.RequestsInterface;
using TheRobot.DriverService;

namespace TheRobot
{
    public class Robot : IRobot
    {
        private readonly ILogger<Robot> _logger;
        private readonly IMediator _mediator;

        public Robot(ILogger<Robot> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<RobotResponse> Execute(IRobotRequest request)
        {
            //request.logger = _logger;

            //if (request.Timeout == null)
            //{
            //    request.Timeout = TimeSpan.FromSeconds(5);
            //}

            //_logger.LogInformation("About to execute {@IRoboRequest}", request);
            //RobotResponse response = new();

            //try
            //{
            //    if (request.Timeout == null)
            //    {
            //        request.Timeout = TimeSpan.FromSeconds(5);
            //    }

            //    request.PreExecute?.Invoke(_driver);

            //    if (request.DelayBefore.Ticks > 0)
            //    {
            //        await Task.Delay(request.DelayBefore);
            //    }

            //    response = request.Exec(_driver);

            //    if (response.Status != RobotResponseStatus.ActionRealizedOk)
            //    {
            //        _logger.LogInformation("The request was not successfully");
            //    }

            //    if (request.DelayAfter.Ticks > 0)
            //    {
            //        await Task.Delay(request.DelayAfter);
            //    }

            //    request.PostExecute?.Invoke(_driver);
            //}
            //catch (Exception ex) when (ExecuteExceptionFilter(ex))
            //{
            //    _logger.LogInformation("An exception was thrown in the request execution.\nThe exception: {@Exception}", ex);
            //    response.Status = RobotResponseStatus.ExceptionOccurred;
            //    response.ErrorMessage = ex.Message;
            //}
            //catch (WebDriverException ex)
            //{
            //    _logger.LogError("An critical exception occurs at the robot driver.\nThe exception: {@Exception}", ex);
            //    response.Status = RobotResponseStatus.ExceptionOccurred;
            //    response.ErrorMessage = ex.Message;
            //}

            //_logger.LogInformation("{@IRoboRequest} Executed", request);
            return new()
            {
                Status = RobotResponseStatus.ActionRealizedOk
            };
        }

        public async Task Exec3Async(IWebRobotRequest<RobotResponse> request)
        {
            //_logger.LogInformation(request.GeneralParameters!.DelayBefore.ToString());

            await _mediator.Send(request);
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