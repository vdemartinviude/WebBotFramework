namespace TheRobot.MediatedRequests;

public class MediatedWaitElementExistOrVanish : GenericMediatedRequest
{
    public bool? WaitForVanish { get; set; }
}