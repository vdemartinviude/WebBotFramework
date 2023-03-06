using MediatR;
using OneOf;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using TheRobot.DriverService;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot.Handles;

public class HandleMediatedClickRequest : IRequestHandler<MediatedClickRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly WebDriverService _webDriverService;

    public HandleMediatedClickRequest(WebDriverService webDriverService)
    {
        _webDriverService = webDriverService;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(MediatedClickRequest request, CancellationToken cancellationToken)
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

        var actionresult = await Task.Run(() => _webDriverService.Click(request.BaseParameters.TimeOut, request.BaseParameters.By, request.Kind, cancellationToken));
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