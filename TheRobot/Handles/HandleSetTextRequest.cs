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

public class HandleSetTextRequest : IRequestHandler<MediatedSetTextRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _driverService;

    public HandleSetTextRequest(WebDriverService driverService)
    {
        _driverService = driverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedSetTextRequest request, CancellationToken cancellationToken)
    {
        return await _driverService.SetText(request.KindOfSetText, request.TextToSet, request.BaseParameters.TimeOut, request.BaseParameters.By, request.numberOfBackSpaces, cancellationToken);
    }
}