using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace RobotTests.Fixtures;

public class RobotFixtures : IDisposable
{
    public readonly Robot Robot;
    public readonly IHttpClientFactory HttpCientFactory;

    public RobotFixtures()
    {
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHttpClient();
            }).Build();
        HttpCientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
        Robot = new Robot(HttpCientFactory);
    }

    public void Dispose()
    {
        Robot.Dispose();
    }
}