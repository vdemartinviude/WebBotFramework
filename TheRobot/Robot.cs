using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using OpenQA.Selenium;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot
{
    public class Robot : IRobot
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public Robot(ILogger<Robot> logger, IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Execute(GenericMediatedRequest request, CancellationToken cancellationToken)
        {
            OneOf<ErrorOnWebAction, SuccessOnWebAction>? result = null;
            if (request.BaseParameters == null)
            {
                int defaultTime = _configuration.GetSection("RobotConfiguration").GetValue<int>("")
                request.BaseParameters = new GenericMediatedParameters
                {
                    TimeOut = TimeSpan.FromSeconds(5)
                };
            }
            if (request.BaseParameters.TimeOut == TimeSpan.Zero)
            {
                request.BaseParameters.TimeOut = TimeSpan.FromSeconds(5);
            }

            if (request.BaseParameters.DelayBefore != TimeSpan.Zero)
            {
                await Task.Delay(request.BaseParameters.DelayBefore, cancellationToken);
            }
            result = await _mediator.Send(request, cancellationToken);
            if (request.BaseParameters.DelayAfter != TimeSpan.Zero)
            {
                await Task.Delay(request.BaseParameters.DelayAfter, cancellationToken);
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