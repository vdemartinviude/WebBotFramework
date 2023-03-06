using MediatR;
using OneOf;
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

public class HandleElementExistsRequest : IRequestHandler<MediatedElementExistsRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _driverService;

    public HandleElementExistsRequest(WebDriverService driverService)
    {
        _driverService = driverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedElementExistsRequest request, CancellationToken cancellationToken)
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

        OneOf<ErrorOnWebAction, SuccessOnWebAction> result = await _driverService.ElementExists(request.BaseParameters.TimeOut, request.BaseParameters.By, cancellationToken);
        stopwatch.Stop();
        if (result.IsT1) { result.AsT1.ElapsedTime = stopwatch.Elapsed; }
        return result;
    }
}