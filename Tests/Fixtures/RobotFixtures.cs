using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;
using TheRobot;
using TheRobot.DriverService;
using TheRobot.PipelineExceptionHandler;

namespace RobotTests.Fixtures;

public class RobotFixtures : IDisposable
{
    public readonly Robot Robot;
    public readonly ILogger<Robot> Logger;
    public readonly CancellationTokenSource TokenSource;

    public RobotFixtures()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/robtotest.log")
            .CreateLogger();

        IHost host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(x => x.AddJsonFile(@"Configuration\RobotConfiguration.json"))
            .ConfigureServices(services =>
            {
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("TheRobot")));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorPipelineBehavior<,>));
                services.AddSingleton<WebDriverService>();
            })
            .UseSerilog()
            .Build();
        var thelogger = host.Services.GetRequiredService<ILogger<Robot>>();
        Robot = new Robot(
            host.Services.GetRequiredService<IMediator>(),
            host.Services.GetRequiredService<WebDriverService>(),
            host.Services.GetRequiredService<IConfiguration>());
        TokenSource = new();
    }

    public void Dispose()
    {
        Robot.Dispose();
    }
}