using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using TheRobot.Responses;

namespace TheRobot.PipelineExceptionHandler;

public class MediatorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    private readonly ILogger<MediatorPipelineBehavior<TRequest, TResponse>> _logger;

    public MediatorPipelineBehavior(ILogger<MediatorPipelineBehavior<TRequest, TResponse>> logger)
    {
        this._logger = logger;
    }

    public async Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Handle(TRequest request, RequestHandlerDelegate<OneOf<ErrorOnWebAction, SuccessOnWebAction>> next, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Executing: {@request}", request);
            var response = await next();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Robot not treated an exception!");

            var result = new ErrorOnWebAction
            {
                SeverityLevel = SeverityLevel.Critical,
                Error = ex.Message
            };
            return result;
        }
    }
}