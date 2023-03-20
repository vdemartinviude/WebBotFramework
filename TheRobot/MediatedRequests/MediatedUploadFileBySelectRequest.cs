using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheRobot.MediatedRequests;

public class MediatedUploadFileBySelectRequest : GenericMediatedRequest
{
    public string FilePath { get; set; }
}