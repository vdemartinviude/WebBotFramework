using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.MediatedRequests;
using TheRobot.Response;
using TheRobot.DriverService;
using TheRobot.WebRequestsParameters;

namespace TheRobot.Handles;

public class HandleNavigationRequest : IRequestHandler<MediatedNavigationRequest, RobotResponse>
{
    private readonly WebDriverService _webDriverService;

    public HandleNavigationRequest(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
    }

    public async Task<RobotResponse> Handle(MediatedNavigationRequest request, CancellationToken cancellationToken)
    {
        await Task.Run(() => _webDriverService.WebDriver.Navigate().GoToUrl(((NavigateRequestParameters)request!.Parameters).Url), cancellationToken);
        return new RobotResponse()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}