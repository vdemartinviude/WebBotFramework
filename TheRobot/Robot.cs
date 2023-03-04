using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
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
                try
                {
                    var wait = new WebDriverWait(_driverService.GetWebDriver(), TimeSpan.FromMilliseconds(100));
                    var element = wait.Until(d => d.FindElement(request.BaseParameters.By));
                    new Actions(_driverService.GetWebDriver())
                        .ScrollToElement(element)
                        .Perform();
                    request.BaseParameters.ElementAlreadyFound = element;
                }
                catch (Exception _)
                {
                }
            }

            result = await _mediator.Send(request, cancellationToken);

            if (request.BaseParameters != null && request.BaseParameters.By != null && result.Value.IsT1 && result.Value.AsT1.WebElement != null)
            {
                try
                {
                    _driverService.GetWebDriver().ExecuteScript("arguments[0].style.background='" + markdowncolor + "';", result.Value.AsT1.WebElement);
                }
                catch (Exception _)
                {
                }
            }

            if (request.BaseParameters!.DelayAfter != TimeSpan.Zero)
            {
                await Task.Delay(request.BaseParameters.DelayAfter, cancellationToken);
            }
            if (result.Value.IsT1)
            {
                result.Value.AsT1.CurrentUrl = _driverService.GetWebDriver().Url;
                result.Value.AsT1.CurrentPageTitle = _driverService.GetWebDriver().Title;
            }
            return result!.Value;
        }
    }
}