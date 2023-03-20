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

public class HandleUploadFileBySelect : IRequestHandler<MediatedUploadFileBySelectRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _driverService;

    public HandleUploadFileBySelect(WebDriverService driverService)
    {
        _driverService = driverService;
    }

    public Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedUploadFileBySelectRequest request, CancellationToken cancellationToken)
    {
        return _driverService.UploadFileBySelect(request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, request.FilePath, cancellationToken);
    }
}