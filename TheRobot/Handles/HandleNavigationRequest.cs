using MediatR;
using TheRobot.MediatedRequests;
using TheRobot.Response;
using TheRobot.DriverService;
using TheRobot.WebRequestsParameters;
using Microsoft.Extensions.Logging;

namespace TheRobot.Handles;

public class HandleNavigationRequest : IRequestHandler<MediatedNavigationRequest, RobotResponse>
{
    private readonly WebDriverService _webDriverService;
    private readonly ILogger<HandleNavigationRequest> _looger;

    public HandleNavigationRequest(WebDriverService webDriverService, ILogger<HandleNavigationRequest> looger)
    {
        _webDriverService = webDriverService;
        _looger = looger;
    }

    public async Task<RobotResponse> Handle(MediatedNavigationRequest request, CancellationToken cancellationToken)
    {
        _looger.LogWarning("Teste do logger");
        await Task.Run(() => _webDriverService.GetWebDriver().Navigate().GoToUrl(((NavigateRequestParameters)request!.Parameters).Url), cancellationToken);
        return new RobotResponse()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}