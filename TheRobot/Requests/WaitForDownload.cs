using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot.FolderMonitorHelper;
using TheRobot.Response;

namespace TheRobot.Requests;

public class WaitForDownload : IRobotRequest
{
    public TimeSpan DelayBefore { get; set; }
    public TimeSpan DelayAfter { get; set; }
    public Action<IWebDriver>? PreExecute { get; set; }
    public Action<IWebDriver>? PostExecute { get; set; }
    public string? Folder { get; set; }
    public List<string>? FileTypes { get; set; }
    public TimeSpan? Timeout { get; set; }
    public CancellationToken? CancellationToken { get; set; }
    public ILogger<Robot>? logger { get; set; }

    public RobotResponse Exec(IWebDriver driver)
    {
        if (Folder is null)
        {
            throw new ArgumentNullException(nameof(Folder));
        }
        if (FileTypes is null)
        {
            throw new ArgumentNullException(nameof(FileTypes));
        }

        if (Timeout is null)
        {
            Timeout = new TimeSpan(0, 5, 0);
        }
        FolderMonitor folderMonitor = new FolderMonitor(Folder);

        var fileData = folderMonitor.WaitForDownloadByFileTypes(FileTypes, (TimeSpan)Timeout);
        if (fileData.TimedOut)
        {
            return new RobotResponse()
            {
                Status = RobotResponseStatus.TimedOut
            };
        }
        var resp = new RobotResponse
        {
            Status = RobotResponseStatus.ActionRealizedOk,
            Data = Path.Combine(Folder, fileData.Name!)
        };
        return resp;
    }
}