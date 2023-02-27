using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.Requests;
using TheRobot.Response;

namespace TheRobot;

public interface IRobot
{
    public abstract Task<RobotResponse> Execute(IRobotRequest request);
}