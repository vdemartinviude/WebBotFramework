using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using System.Diagnostics;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleNavigationRequest : IRequestHandler<MediatedNavigationRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _webDriverService;

    public HandleNavigationRequest(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
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