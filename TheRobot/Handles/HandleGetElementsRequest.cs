using MediatR;
using OneOf;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleGetElementsRequest : IRequestHandler<MediatedGetElementsRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _driverService;

    public HandleGetElementsRequest(WebDriverService driverService)
    {
        _driverService = driverService;
    }

    public Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedGetElementsRequest request, CancellationToken cancellationToken)
    {
        return _driverService.GetElements(request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, cancellationToken);
    }
}