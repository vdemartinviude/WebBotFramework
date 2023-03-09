using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleGetShadowRootRequest : IRequestHandler<MediatedGetShadowRoot, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _driverService;

    public HandleGetShadowRootRequest(WebDriverService driverService)
    {
        _driverService = driverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedGetShadowRoot request, CancellationToken cancellationToken)
    {
        return await _driverService.GetShadow(request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, cancellationToken);
    }
}