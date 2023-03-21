using MediatR;
using OneOf;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleWaitExistsOrVanishRequest : IRequestHandler<MediatedWaitElementExistOrVanish, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private WebDriverService _driverService { get; set; }

    public HandleWaitExistsOrVanishRequest(WebDriverService driverService)
    {
        _driverService = driverService;
    }

    public Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedWaitElementExistOrVanish request, CancellationToken cancellationToken)
    {
        return _driverService.WaitExistOrVanish(request.BaseParameters.TimeOut, request.BaseParameters.ByOrElement, request.WaitForVanish, cancellationToken);
    }
}