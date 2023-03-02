using MediatR;
using OneOf;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TheRobot.RequestsInterface;
using TheRobot.Response;
using TheRobot.Responses;
using TheRobot.WebRequestsParameters;

namespace TheRobot.MediatedRequests;

public class MediatedNavigationRequest : IWebRobotRequest<OneOf<ErrorOnWebAction, SuccessOnWebAction>>
{
    public IWebBotRequestParameter Parameters { get; set; }
}