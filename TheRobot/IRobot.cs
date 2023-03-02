using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Requests;
using TheRobot.RequestsInterface;
using TheRobot.Response;

namespace TheRobot;

public interface IRobot
{
    public abstract Task Exec3Async(IWebRobotRequest<RobotResponse> request, CancellationToken cancellationToken);
}