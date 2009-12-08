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
using System.Windows.Controls.Primitives;

namespace SpokenBible.View
{
    /// <summary>
    /// Interaction logic for Shortcuts.xaml
    /// </summary>
    public partial class Shortcuts : Page
    {
        private MainPresenter presenter = null;

        public IEnumerable<Testamento> Testamentos
        {
            set
            {
                foreach (Testamento testamento in value)
                {
                    TabItem newTab = new TabItem();
                    newTab.Header = testamento.Display;
                    tabs.Items.Insert(tabs.Items.Count-1, newTab);

                    UniformGrid gridExterno = new UniformGrid();
                    gridExterno.Columns = 3;
                    gridExterno.Margin = new Thickness(0, 10, 0, 10);

                    int total = testamento.Livros.Count;
                    int totalPorColuna = testamento.Livros.Count / 3;
                    for (int passo = 0; passo < 3; passo++)
                    {
                        Border bordaInterna = new Border();
                        bordaInterna.BorderBrush = Brushes.Gray;
                        bordaInterna.Padding = new Thickness(10);
                        bordaInterna.BorderThickness = new Thickness(passo == 0 ? 0 : 1, 0, 0, 0);

                        UniformGrid gridInterno = new UniformGrid();
                        gridInterno.Columns = 1;

                        int fim = passo < 3 ? (passo + 1) * totalPorColuna : total;
                        for (int i = passo * totalPorColuna; i < fim; i++)
                        {
                            Label label = new Label();
                            label.Content = testamento.Livros[i];
                            label.KeyUp += label_KeyUp;
                            label.MouseUp += label_MouseUp;

                            gridInterno.Children.Add(label);
                        }

                        bordaInterna.Child = gridInterno;
                        gridExterno.Children.Add(bordaInterna);
                    }

                    newTab.Content = gridExterno;
                }
            }
        }

        public IEnumerable<Traducao> Traducoes
        {
            set
            {
                cbTraducoes.ItemsSource = value;
                cbTraducoes.DisplayMemberPath = "Display";
            }
        }

        public Shortcuts(MainPresenter presenter)
        {
            this.presenter = presenter;
            InitializeComponent();
        }

        private void showHide(object sender, RoutedEventArgs e)
        {
            this.presenter.ShowHideShortcuts();
        }

        private void label_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                this.ItemSelecionado(sender);
        }

        private void label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ItemSelecionado(sender);
        }

        private void ItemSelecionado(object sender)
        {
            Label label = (Label)sender;
            Livro livro = (Livro)label.Content;
            this.presenter.ShowHideShortcuts();
            this.presenter.ShowContentFromShortcuts(livro);
        }

        private void btAddTraducao_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "Selecione o arquivo que contém a tradução desejada.";
            dialog.Filter = "Arquivo de Tradução|*.sbt";
            if (dialog.ShowDialog() == true)
            {
                this.presenter.AdicionarTraducao(dialog.FileName);
            }
        }

    }
}
