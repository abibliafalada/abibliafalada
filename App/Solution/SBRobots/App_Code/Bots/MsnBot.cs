using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSNPSharp;

namespace SBRobots.Bots
{
    public class MsnBot : IBot
    {
        private Messenger messenger = null;

        public event BotMessageHandler OnMessage;

        public BotStatus Status { get { return messenger.Connected ? BotStatus.Active : BotStatus.Inactive; } }
        public string CustomMessage { get; set; }

        public string BotDomain { get; set; }
        public string BotServer { get; set; }
        public string BotUserName { get; set; }
        public string BotPassword { get; set; }

        public MsnBot()
        {
            this.messenger = new Messenger();
        }

        public void Start()
        {
            if (messenger.Connected)
                return;

            messenger.Nameserver.OIMService.OIMReceived += new EventHandler<OIMReceivedEventArgs>(Nameserver_OIMReceived);
            messenger.Credentials = new Credentials(this.BotUserName, this.BotPassword, MsnProtocol.MSNP18);

            messenger.Connect();
        }

        public void Stop()
        {
            if (messenger.Connected)
                messenger.Disconnect();
        }

        void Nameserver_OIMReceived(object sender, OIMReceivedEventArgs e)
        {
            /*if (InvokeRequired)
            {
                Invoke(new EventHandler<OIMReceivedEventArgs>(Nameserver_OIMReceived), sender, e);
                return;
            }

            if (DialogResult.Yes == MessageBox.Show(
                "OIM received at : " + e.ReceivedTime + "\r\nFrom : " + e.NickName + " (" + e.Email + ") " + ":\r\n"
                + e.Message + "\r\n\r\n\r\nClick yes, if you want to receive this message next time you login.",
                "Offline Message from " + e.Email, MessageBoxButtons.YesNoCancel))
            {
                e.IsRead = false;
            }*/

        }
    }
}
