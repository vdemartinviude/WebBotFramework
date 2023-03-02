using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.WebRequestsParameters;

namespace TheRobot.RequestsInterface;

public interface IWebRobotRequest<T> : IRequest<T>
{
    public IWebBotRequestParameter Parameters { get; set; }
}