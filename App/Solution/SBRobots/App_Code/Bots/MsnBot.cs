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
            messenger.Nameserver.SignedIn += new EventHandler<EventArgs>(Nameserver_SignedIn);
            messenger.Nameserver.ExceptionOccurred += new EventHandler<ExceptionEventArgs>(Nameserver_ExceptionOccurred);
            messenger.Nameserver.AuthenticationError += new EventHandler<ExceptionEventArgs>(Nameserver_AuthenticationError);
            messenger.ContactService.ServiceOperationFailed += new EventHandler<ServiceOperationFailedEventArgs>(ContactService_ServiceOperationFailed);
            messenger.OIMService.ServiceOperationFailed += new EventHandler<ServiceOperationFailedEventArgs>(OIMService_ServiceOperationFailed);
            messenger.StorageService.ServiceOperationFailed += new EventHandler<ServiceOperationFailedEventArgs>(StorageService_ServiceOperationFailed);
            messenger.WhatsUpService.ServiceOperationFailed += new EventHandler<ServiceOperationFailedEventArgs>(WhatsUpService_ServiceOperationFailed);
            messenger.Nameserver.WhatsUpService.GetWhatsUpCompleted += new EventHandler<GetWhatsUpCompletedEventArgs>(WhatsUpService_GetWhatsUpCompleted);

            messenger.Credentials = new Credentials(this.BotUserName, this.BotPassword, MsnProtocol.MSNP18);

            messenger.Connect();
        }

        void WhatsUpService_GetWhatsUpCompleted(object sender, GetWhatsUpCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void WhatsUpService_ServiceOperationFailed(object sender, ServiceOperationFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void StorageService_ServiceOperationFailed(object sender, ServiceOperationFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void OIMService_ServiceOperationFailed(object sender, ServiceOperationFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void ContactService_ServiceOperationFailed(object sender, ServiceOperationFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Nameserver_AuthenticationError(object sender, ExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Nameserver_ExceptionOccurred(object sender, ExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Nameserver_SignedIn(object sender, EventArgs e)
        {
            messenger.Owner.Status = (PresenceStatus)Enum.Parse(typeof(PresenceStatus), "altz");
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
