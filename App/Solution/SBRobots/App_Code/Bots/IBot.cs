using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SBRobots.Bots
{
    public delegate string BotMessageHandler(object sender, string term);

    public enum BotStatus
    {
        Inactive, Active
    }

    public interface IBot
    {
        event BotMessageHandler OnMessage;

        BotStatus Status { get; }

        string CustomMessage { get; set; }

        string BotDomain { get; set; }
        string BotServer { get; set; }
        string BotUserName { get; set; }
        string BotPassword { get; set; }

        void Start();
        void Stop();
    }
}
