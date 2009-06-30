using System;
using System.Linq;
using System.Text;
using System.Windows;
using SpokenBible.View;
using sbcore.Persistence;
using sbcore.Model;
using System.Collections;
using sbcore.Model.Interface;

namespace SpokenBible.Presenter
{
    public class MainPresenter
    {
        MainWindow window = null;

        public MainPresenter()
        {
            window = new MainWindow(this);
        }

        public void ShowView()
        {
            window.Show();
        }

        public void ShowContent(ISbItem item)
        {
            window.ShowContent(item);
            
        }

    }
}
