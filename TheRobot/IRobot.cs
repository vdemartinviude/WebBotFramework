using OneOf;
using TheRobot.MediatedRequests;
using TheRobot.Responses;

namespace TheRobot;

public interface IRobot
{
    public abstract Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Execute(GenericMediatedRequest request, CancellationToken cancellationToken);
}