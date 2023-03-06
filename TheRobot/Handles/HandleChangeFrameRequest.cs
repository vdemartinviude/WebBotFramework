using MediatR;
using OneOf;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleChangeFrameRequest : IRequestHandler<MediatedChangeFrameRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _webDriverService;

    public HandleChangeFrameRequest(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedChangeFrameRequest request, CancellationToken cancellationToken)
    {
        if (request.BaseParameters == null)
        {
            throw new ArgumentNullException("BaseParameters");
        }
        if (request.BaseParameters.By == null)
        {
            throw new ArgumentNullException("By");
        }
        Stopwatch stopwatch = Stopwatch.StartNew();

        var actionresult = await Task.Run(() => _webDriverService.ChangeFrame(request.BaseParameters.TimeOut, request.BaseParameters.By, cancellationToken));
        if (actionresult.IsT1)
        {
            stopwatch.Stop();
            actionresult.AsT1.ElapsedTime = stopwatch.Elapsed;
        }
        else
        {
            stopwatch.Stop();
        }
        return actionresult;
    }
}