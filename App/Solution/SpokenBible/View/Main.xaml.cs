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
using sbcore.Model.Interface;
using sbcore.Model;

namespace SpokenBible.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        MainPresenter presenter = null;

        public Main(MainPresenter presenter)
        {
            this.presenter = presenter;
            InitializeComponent();
        }

        public void ClearContent()
        {
            documentReader.Document.Blocks.Clear();
            FlowDocument trickToMoveToBegin = documentReader.Document;
            documentReader.Document = null;
            documentReader.Document = trickToMoveToBegin;
        }

        public void ShowContent(IEnumerable<ISbItem> itens)
        {
            ShowFather(itens.First().Parent);
            foreach (ISbItem item in itens)
            {
                ShowContent(item);
            }
        }

        private void ShowContent(ISbItem item)
        {
            CreateParagraph(item);

            foreach (ISbItem i in item.Children)
                ShowContent(i);
        }

        private void ShowFather(ISbItem item)
        {
            if ((item == null) || (item is Testamento))
                return;

            ShowFather(item.Parent);
            CreateParagraph(item);
        }

        private void CreateParagraph(ISbItem item)
        {
            Paragraph p = new Paragraph();
            FormatParagraph(p, item);
            p.Cursor = Cursors.Hand;
            p.Inlines.Add(item.Display);
            p.MouseLeave += new MouseEventHandler(OnParagraphMouseLeave);
            p.MouseEnter += new MouseEventHandler(OnParagraphMouseEnter);
            p.MouseDown += new MouseButtonEventHandler(OnParagraphMouseDown);
            documentReader.Document.Blocks.Add(p);
        }

        private void FormatParagraph(Paragraph p, ISbItem item)
        {
            if(item is Livro)
                p.FontSize = 28;
            else if (item is Capitulo)
                p.FontSize = 24;
        }

        private void OnParagraphMouseEnter(object sender, MouseEventArgs e)
        {
            Paragraph p = sender as Paragraph;
            p.Background = Brushes.LightGray;
        }

        private void OnParagraphMouseLeave(object sender, MouseEventArgs e)
        {
            Paragraph p = sender as Paragraph;
            p.Background = null;
        }

        private void OnParagraphMouseDown(object sender, MouseButtonEventArgs e)
        {
            Paragraph p = sender as Paragraph;
            TextRange textRange = new TextRange(p.ContentStart, p.ContentEnd);
            if (textRange.Text != string.Empty)
                this.presenter.SpeachRequest(textRange.Text);
        }

        private void busca_TextChanged(object sender, RoutedEventArgs e)
        {
            this.presenter.SearchChanged(busca, busca.Text);
        }

        private void busca_SearchRequest(object sender, RoutedEventArgs e)
        {
            this.presenter.SearchRequested(busca.Text);
        }

        private void ler_Click(object sender, RoutedEventArgs e)
        {
            if (this.documentReader.Selection.Text != string.Empty)
            {
                this.presenter.SpeachRequest(this.documentReader.Selection.Text);
            }
            else
            {
                TextRange r = new TextRange(this.documentReader.Document.Blocks.FirstBlock.ContentStart, this.documentReader.Document.Blocks.LastBlock.ContentEnd);
                this.presenter.SpeachRequest(r.Text);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.ler);
        }
    }
}
