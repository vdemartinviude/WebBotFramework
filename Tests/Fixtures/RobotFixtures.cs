using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TheRobot;
using Serilog;

namespace RobotTests.Fixtures;

public class RobotFixtures : IDisposable
{
    public readonly Robot Robot;
    public readonly IHttpClientFactory HttpCientFactory;
    public readonly ILogger<Robot> Logger;

    public RobotFixtures()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("/logs/robtotest.log")
            .CreateLogger();

        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHttpClient();
            })
            .UseSerilog()
            .Build();
        HttpCientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
        Logger = host.Services.GetRequiredService<ILogger<Robot>>();
        Robot = new Robot(HttpCientFactory, Logger);
    }

    public void Dispose()
    {
        Robot.Dispose();
    }
}