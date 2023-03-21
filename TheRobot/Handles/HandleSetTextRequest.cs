using MediatR;
using OneOf;
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
        return await _driverService.SetText(request.KindOfSetText, request.TextToSet, request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, request.numberOfBackSpaces, cancellationToken);
    }
}