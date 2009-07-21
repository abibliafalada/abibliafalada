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
using sbcore.Model;
using sbcore.Model.Interface;
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

        public void ShowContent(ISbItem item)
        {
            foreach (ISbItem i in item.Children)
            {
                Paragraph p = new Paragraph();
                p.Inlines.Add(i.Display);
                document.Blocks.Add(p);
                ShowContent(i);
            }
        }

        public IList<Livro> AutoCompleteItems
        {
            set
            {
                /*busca.Items.Clear();
                busca.DisplayMemberPath = "Nome";
                
                busca.ItemsSource = value;
                busca.BringIntoView();*/
                //busca.Items.Clear();

                /*foreach (Livro l in value)
                {
                    busca.AddItem(new AutoCompleteEntry(l.Nome, l.Acronimo, l.Nome));
                }*/
            }
        }

        private void busca_TextChanged(object sender, RoutedEventArgs e)
        {
            IList<string> itens = new List<string>();
            itens.Add(busca.Text);
            itens.Add(busca.Text + " 1");
            itens.Add(busca.Text + " 2");
            busca.ItemsSource = itens;
        }

    }
}
