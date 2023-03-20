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

public class HandleSelectRequest : IRequestHandler<MediatedSelectRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _driverService;

    public HandleSelectRequest(WebDriverService driverService)
    {
        _driverService = driverService;
    }

    public Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedSelectRequest request, CancellationToken cancellationToken)
    {
        switch (request.KindOfSelect)
        {
            case KindOfSelect.SelectByDriveByText:
                return _driverService.SelectByDriveByText(request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, request.Text, cancellationToken);

            default:
                return _driverService.SelectByDriveByValue(request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, request.Value, cancellationToken);
        }
    }
}