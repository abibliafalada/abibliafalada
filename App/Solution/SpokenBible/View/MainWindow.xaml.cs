using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpokenBible.Presenter;

namespace SpokenBible.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainPresenter presenter = null;
        
        public MainWindow(MainPresenter presenter)
        {
            this.presenter = presenter;
            InitializeComponent();
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    this.presenter.SpeachStop();
                    break;
                case Key.F1:
                    this.presenter.SpeachRequest("Ajuda");
                    break;
            }
        }
    }
}
