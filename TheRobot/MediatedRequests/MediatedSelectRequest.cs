namespace TheRobot.MediatedRequests;

public enum KindOfSelect
{
    SelectByDrive,
    SelectBy2Clicks
}
public class MediatedSelectRequest : GenericMediatedRequest
{
    public KindOfSelect KindOfSelect { get; set; }
    public GenericWebElement SecondElement { get; set; }    

}