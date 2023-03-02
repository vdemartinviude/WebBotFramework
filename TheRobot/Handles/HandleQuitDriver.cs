using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Response;
using OneOf;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleQuitDriver : IRequestHandler<MediatedQuitDriverRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _webDriverService;

    public HandleQuitDriver(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedQuitDriverRequest request, CancellationToken cancellationToken)
    {
        await Task.Run(() => _webDriverService.GetWebDriver().Quit(), cancellationToken);
        return new SuccessOnWebAction()
        {
            ElapsedTime = TimeSpan.FromSeconds(0)
        };
    }
}