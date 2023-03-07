using MediatR;
using Microsoft.Extensions.Configuration;
using OneOf;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot
{
    public class Robot : IRobot, IDisposable
    {
        private readonly IMediator _mediator;
        private readonly WebDriverService _driverService;
        private readonly IConfiguration _configuration;

        public Robot(IMediator mediator, WebDriverService driverService, IConfiguration configuration)
        {
            _mediator = mediator;
            _driverService = driverService;
            _configuration = configuration;
        }

        public void Dispose()
        {
            _driverService.Dispose();
        }

        public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Execute(GenericMediatedRequest request, CancellationToken cancellationToken)
        {
            int defaultTimeout = _configuration.GetRequiredSection("RobotConfiguration").GetValue<int>("DefaultTimeout");
            string markdowncolor = _configuration.GetRequiredSection("RobotConfiguration").GetValue<string>("MarkdownColor")!;

            OneOf<ErrorOnWebAction, SuccessOnWebAction>? result;
            request.BaseParameters ??= new GenericMediatedParameters
            {
                TimeOut = TimeSpan.FromSeconds(defaultTimeout)
            };
            if (request.BaseParameters.TimeOut == TimeSpan.Zero)
            {
                request.BaseParameters.TimeOut = TimeSpan.FromSeconds(defaultTimeout);
            }

            if (request.BaseParameters.DelayBefore != TimeSpan.Zero)
            {
                await Task.Delay(request.BaseParameters.DelayBefore, cancellationToken);
            }
            if (request.BaseParameters != null && request.BaseParameters.By != null)
            {
                var elementScrolled = await _driverService.ScrollToElement(TimeSpan.FromMilliseconds(100), request.BaseParameters.By, cancellationToken);
                if (elementScrolled.IsT1)
                {
                    request.BaseParameters.ElementAlreadyFound = elementScrolled.AsT1.WebElement;
                }
            }

            result = await _mediator.Send(request, cancellationToken);

            if (request.BaseParameters != null && result.Value.IsT1 && result.Value.AsT1.WebElement != null)
            {
                await _driverService.MarkDownElement(result.Value.AsT1.WebElement, markdowncolor, cancellationToken);
            }

            if (request.BaseParameters!.DelayAfter != TimeSpan.Zero)
            {
                await Task.Delay(request.BaseParameters.DelayAfter, cancellationToken);
            }
            if (result.Value.IsT1)
            {
                result.Value.AsT1.CurrentUrl = _driverService.CurrentUrl;
                result.Value.AsT1.CurrentPageTitle = _driverService.CurrentPageTitle;
            }
            return result!.Value;
        }
    }
}