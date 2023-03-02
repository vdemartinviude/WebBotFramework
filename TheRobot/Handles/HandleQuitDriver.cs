using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Response;

namespace TheRobot.Handles;

public class HandleQuitDriver : IRequestHandler<MediatedQuitDriverRequest, RobotResponse>
{
    private readonly WebDriverService _webDriverService;

    public HandleQuitDriver(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
    }

    public async Task<RobotResponse> Handle(MediatedQuitDriverRequest request, CancellationToken cancellationToken)
    {
        await Task.Run(() => _webDriverService.WebDriver.Quit(), cancellationToken);
        return new()
        {
            Status = RobotResponseStatus.ActionRealizedOk
        };
    }
}