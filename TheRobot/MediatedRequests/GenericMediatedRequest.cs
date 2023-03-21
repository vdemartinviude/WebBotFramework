using MediatR;
using OneOf;
using TheRobot.Responses;

namespace TheRobot.MediatedRequests;

public abstract class GenericMediatedRequest : IRequest<OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    public GenericMediatedParameters BaseParameters { get; set; }
}