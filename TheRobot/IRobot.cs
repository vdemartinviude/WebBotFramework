using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Requests;
using TheRobot.RequestsInterface;
using TheRobot.Response;
using TheRobot.Responses;
using OneOf;

namespace TheRobot;

public interface IRobot
{
    public abstract Task<OneOf<ErrorOnWebAction, SuccessOnWebAction>> Execute(IWebRobotRequest<OneOf<ErrorOnWebAction, SuccessOnWebAction>> request, CancellationToken cancellationToken);
}