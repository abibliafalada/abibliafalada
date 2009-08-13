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
    /// Interaction logic for Shortcuts.xaml
    /// </summary>
    public partial class Shortcuts : Page
    {
        private MainPresenter presenter = null;

        public Shortcuts(MainPresenter presenter)
        {
            this.presenter = presenter;
            InitializeComponent();
        }

        private void ocultar(object sender, RoutedEventArgs e)
        {
            this.presenter.HideShortcuts();
        }

    }
}
