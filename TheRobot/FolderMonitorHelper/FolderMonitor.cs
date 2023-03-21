namespace TheRobot.FolderMonitorHelper;

public class FolderMonitor
{
    private readonly FileSystemWatcher watcher;

    public FolderMonitor(string folderName)
    {
        watcher = new FileSystemWatcher(folderName);
    }

    public WaitForChangedResult WaitForDownloadByFileTypes(List<string> FileTypes, TimeSpan timeout)
    {
        watcher.Filters.Clear();

        FileTypes.ForEach((FileType) => watcher.Filters.Add(FileType));
        return watcher.WaitForChanged(WatcherChangeTypes.All, (int)Math.Ceiling(timeout.TotalMilliseconds));
    }
}