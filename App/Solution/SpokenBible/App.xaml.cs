using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SpokenBible.Controller;

namespace SpokenBible
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        AppController controller = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            controller = new AppController();
            controller.Start();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            controller.End();
        }
    }
}
