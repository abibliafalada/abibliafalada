using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SBRobots.Properties;

namespace SBRobots
{
    public partial class _Default : System.Web.UI.Page
    {
        private Controller controller = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            controller = new Controller(Application);
            update_status();
        }

        protected void btStart_Click(object sender, EventArgs e)
        {
            controller.StartServer();
            update_status();
        }

        protected void btStop_Click(object sender, EventArgs e)
        {
            controller.StopServer();
            update_status();
        }

        private void update_status()
        {
            this.lbStatus.Text = controller.Xmpp.Authenticated ? "Authenticated" : "Not authenticated";
        }
    }
}
