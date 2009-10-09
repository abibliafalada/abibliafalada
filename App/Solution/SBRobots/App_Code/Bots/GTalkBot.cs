using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using agsXMPP;
using agsXMPP.protocol.client;

namespace SBRobots.Bots
{
    public class GTalkBot : IBot
    {
        private XmppClientConnection xmpp = null;

        public event BotMessageHandler OnMessage;

        public BotStatus Status { get { return xmpp.Authenticated ? BotStatus.Active : BotStatus.Inactive; } }
        public string CustomMessage { get; set; }

        public string BotDomain { get; set; }
        public string BotServer { get; set; }
        public string BotUserName { get; set; }
        public string BotPassword { get; set; }

        public GTalkBot()
        {
            this.xmpp = new XmppClientConnection();
        }

        public void Start()
        {
            if (xmpp.Authenticated)
                return;

            xmpp.OnLogin += new ObjectHandler(xmpp_OnLogin);
            xmpp.OnPresence += new PresenceHandler(xmpp_OnPresence);
            xmpp.OnMessage += new agsXMPP.protocol.client.MessageHandler(xmpp_OnMessage);

            xmpp.AutoPresence = false;
            xmpp.Port = 5222;
            xmpp.UseSSL = false;
            xmpp.Server = this.BotDomain;
            xmpp.ConnectServer = this.BotServer;

            xmpp.Open(this.BotUserName, this.BotPassword);
        }

        public void Stop()
        {
            xmpp.Close();
        }

        private void xmpp_OnLogin(object sender)
        {
            Presence p = new Presence();
            p.Status = this.CustomMessage;
            xmpp.Send(p);
        }

        private void xmpp_OnPresence(object sender, Presence pres)
        {
            if (pres.Type == PresenceType.subscribe)
            {
                xmpp.PresenceManager.ApproveSubscriptionRequest(pres.From);
            }
            else if (pres.Type == PresenceType.available)
            {
                //Xmpp.Send(new Message(pres.From, "Versículo de hoje: \"Porque Deus amou ao mundo de tal maneira que deu o seu Filho unigênito, para que todo o que nele crê não pereça, mas tenha a vida eterna.\" João 3:16"));
            }
        }

        private void xmpp_OnMessage(object sender, Message msg)
        {
            if (this.OnMessage != null)
                xmpp.Send(new Message(msg.From, this.OnMessage(this, msg.Body)));
        }
    }
}
