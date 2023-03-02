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
using TheRobot.Responses;
using OneOf;

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

        public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Execute(IWebRobotRequest<OneOf<ErrorOnWebAction, SuccessOnWebAction>> request, CancellationToken cancellationToken)
        {
            OneOf<ErrorOnWebAction, SuccessOnWebAction>? result = null;

            if (request.Parameters == null)
            {
                request.Parameters = new GenericRequestParameter
                {
                    Timeout = TimeSpan.FromSeconds(10)
                };
            }
            if (request.Parameters.Timeout == null)
            {
                request.Parameters.Timeout = TimeSpan.FromSeconds(10);
            }

            if (request.Parameters.DelayBefore != null)
            {
                await Task.Delay(request.Parameters.DelayBefore.Value, cancellationToken);
            }
            result = await _mediator.Send(request, cancellationToken);
            if (request.Parameters.DelayAfter != null)
            {
                await Task.Delay(request.Parameters.DelayAfter.Value, cancellationToken);
            }
            return result!.Value;
        }

        private bool ExecuteExceptionFilter(Exception ex)
        {
            return ex is NoSuchElementException ||
                   ex is WebDriverTimeoutException ||
                   ex is NoSuchFrameException ||
                   ex is NoSuchWindowException ||
                   ex is WebDriverException;
        }
    }
}