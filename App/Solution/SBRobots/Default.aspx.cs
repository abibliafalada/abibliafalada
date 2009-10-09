using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SBRobots.Properties;
using SBRobots.Bots;

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
            controller.StartAll();
            update_status();
        }

        protected void btStop_Click(object sender, EventArgs e)
        {
            controller.StopAll();
            update_status();
        }

        private void update_status()
        {
            if (controller.Bots == null)
            {
                this.lbStatus.Text = "Not initialized";
                return;
            }

            this.lbStatus.Text = string.Empty;
            foreach(IBot bot in controller.Bots)
            {
                this.lbStatus.Text += bot.Status == BotStatus.Active ? "Active, " : "Inactive, ";
            }
        }
    }
}
