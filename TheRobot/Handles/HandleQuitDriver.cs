using MediatR;
using OneOf;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleQuitDriver : IRequestHandler<MediatedQuitDriverRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _webDriverService;

    public HandleQuitDriver(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedQuitDriverRequest request, CancellationToken cancellationToken)
    {
        return await Task.Run(() => _webDriverService.QuitDriver(), cancellationToken);
    }
}