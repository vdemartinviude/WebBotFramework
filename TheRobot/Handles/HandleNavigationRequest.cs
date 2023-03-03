using MediatR;
using TheRobot.MediatedRequests;
using TheRobot.Response;
using TheRobot.DriverService;
using TheRobot.WebRequestsParameters;
using Microsoft.Extensions.Logging;
using TheRobot.Responses;
using System.Diagnostics;
using OneOf;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TheRobot.Handles;

public class HandleNavigationRequest : IRequestHandler<MediatedNavigationRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _webDriverService;
    private readonly ILogger<HandleNavigationRequest> _looger;

    public HandleNavigationRequest(WebDriverService webDriverService, ILogger<HandleNavigationRequest> looger)
    {
        _webDriverService = webDriverService;
        _looger = looger;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedNavigationRequest request, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        await Task.Run(() => _webDriverService.GetWebDriver().Navigate().GoToUrl(request.Url), cancellationToken);
        return new SuccessOnWebAction
        {
            ElapsedTime = stopwatch.Elapsed
        };
    }
}