using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.MediatedRequests;

public enum KindOfSetText
{
    SetByWebDriver,
    SetWithKeyPress,
    SetWithBackSpaceAndKeyPress,
    SetWithJs
}

public class MediatedSetTextRequest : GenericMediatedRequest
{
    public string TextToSet { get; set; }
    public KindOfSetText KindOfSetText { get; set; }
    public int? numberOfBackSpaces { get; set; }
}