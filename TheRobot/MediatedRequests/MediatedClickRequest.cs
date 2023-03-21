namespace TheRobot.MediatedRequests;

public enum KindOfClik
{
    ClickByDriver,
    ClickByJavaScriptWithoutFocus,
    ClickByJavaScriptWithFocus
}

public class MediatedClickRequest : GenericMediatedRequest
{
    public KindOfClik Kind { get; set; }
}