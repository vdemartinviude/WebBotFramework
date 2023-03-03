using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Responses;
using OneOf;

namespace TheRobot.MediatedRequests;

public abstract class GenericMediatedRequest : IRequest<OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    public GenericMediatedParameters BaseParameters { get; set; }
}