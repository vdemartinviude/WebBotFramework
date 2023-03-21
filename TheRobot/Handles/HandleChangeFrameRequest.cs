﻿using MediatR;
using OneOf;
using System.Diagnostics;
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
        Stopwatch stopwatch = Stopwatch.StartNew();

        var actionresult = await Task.Run(() => _webDriverService.ChangeFrame(request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, cancellationToken));
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