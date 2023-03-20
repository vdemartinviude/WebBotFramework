namespace TheRobot.MediatedRequests;

public enum KindOfSelect
{
    SelectByDriveByValue,
    SelectByDriveByText,
    SelectBy2Clicks
}

public class MediatedSelectRequest : GenericMediatedRequest
{
    public KindOfSelect KindOfSelect { get; set; }
    public GenericWebElement SecondElement { get; set; }
    public string Value { get; set; }
    public string Text { get; set; }
}