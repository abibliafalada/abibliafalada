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
using System.Collections;
using sbcore.Model;

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

        private void showHide(object sender, RoutedEventArgs e)
        {
            this.presenter.ShowHideShortcuts();
        }

        public IEnumerable VelhoTestamento
        {
            set { velhoTestamento.ItemsSource = value; }
        }

        public IEnumerable NovoTestamento
        {
            set { novoTestamento.ItemsSource = value; }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = (ListBox)sender;
            Livro livro = (Livro)list.SelectedItem;
            this.presenter.ShowHideShortcuts();
            this.presenter.ClosePrincipal();
            this.presenter.SearchRequested(livro.ToString());
        }

    }
}
