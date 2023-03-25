using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.DriverService;

public interface IWebDriverServiceNoSeleniun : IDisposable
{
    public abstract Task Start();

    public abstract Task NewSession(CancellationToken token);
}