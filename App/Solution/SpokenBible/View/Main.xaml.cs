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
using System.Windows.Xps.Packaging;


namespace SpokenBible.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        MainPresenter presenter = null;

        private Paragraph LinkABibliaFalada
        {
            get
            {
                Paragraph paragraphABibliaFalada = new Paragraph();
                paragraphABibliaFalada.TextAlignment = TextAlignment.Right;

                Hyperlink linkABibliaFalada = new Hyperlink(new Run("A Bíblia Falada"));
                linkABibliaFalada.RequestNavigate += HandleRequestNavigate;
                linkABibliaFalada.NavigateUri = new Uri(this.presenter.LinkSite);

                paragraphABibliaFalada.Inlines.Add(linkABibliaFalada);

                return paragraphABibliaFalada;
            }
        }
        

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

        public void ShowHelp()
        {
            ClearContent();

            Thickness margemTitulo = new Thickness(5, 20, 5, 5);
            Thickness margemParagrafo = new Thickness(5);

            Hyperlink linkRaquel = new Hyperlink(new Run("Raquel SAPI5 4shared"));
            linkRaquel.RequestNavigate += HandleRequestNavigate;
            linkRaquel.NavigateUri = new Uri("http://www.4shared.com/file/35334181/7d07e5b1/realspeak_-_raquel_-_sapi5_-_portugus_brasil.html");

            Paragraph t1a = new Paragraph();
            
            Paragraph t2a = new Paragraph();
            Paragraph t2b = new Paragraph();

            Paragraph ta = new Paragraph();
            Paragraph tb = new Paragraph();
            Paragraph tc = new Paragraph();
            Paragraph td = new Paragraph();
            Paragraph te = new Paragraph();
            Paragraph tf = new Paragraph();
            
            t1a.FontSize = 26;
            t1a.Margin = margemTitulo;
            
            t2a.FontSize = 24;
            t2a.Margin = margemTitulo;
            t2b.FontSize = 24;
            t2b.Margin = margemTitulo;

            ta.Margin = margemParagrafo;
            tb.Margin = margemParagrafo;
            tc.Margin = margemParagrafo;
            td.Margin = margemParagrafo;
            te.Margin = margemParagrafo;
            tf.Margin = margemParagrafo;
            
            t1a.Inlines.Add("Ajuda da Bíblia Falada 2.0 – Gênesis");
            
            t2a.Inlines.Add("Como procurar por uma passagem específica?");
            t2b.Inlines.Add("Não consegue ouvir os textos em português?");
            
            ta.Inlines.Add(new Run("Envie suas sugestões para: "));
            ta.Inlines.Add(new Run("sugestoes@abibliafalada.com.br"));

            tb.Inlines.Add(new Run("Além de selecionar um livro acessando o menu à esquerda, você também pode utilizar as formas mais comuns de referenciar os textos bíblicos para encontrar as passagens desejadas."));

            tc.Inlines.Add(new Run("Por exemplo, para encontrar o Salmo de número 23, basta digitar no campo de busca:"));
            tc.Inlines.Add(new LineBreak());
            tc.Inlines.Add(new Bold(new Run("Salmo, 23 ")));
            tc.Inlines.Add(new Run("ou simplesmente: "));
            tc.Inlines.Add(new Bold(new Run("Sl23")));

            td.Inlines.Add(new Run("Também é possível buscar um livro completo: "));
            td.Inlines.Add(new Bold(new Run("João")));

            te.Inlines.Add(new Run("Ou mesmo alguns versículos de um capítulo específico: "));
            te.Inlines.Add(new Bold(new Run("João, 3.16-18")));

            tf.Inlines.Add(new Run("Para ouvir os textos em português é necessário instalar uma voz neste idioma. Você deve baixar e instalar a voz "));
            tf.Inlines.Add(new Bold(new Run("Raquel")));
            tf.Inlines.Add(new Run(", procure no Google ou baixe em: "));
            tf.Inlines.Add(new LineBreak());
            tf.Inlines.Add(linkRaquel);
            tf.Inlines.Add(new LineBreak());
            
            documentReader.Document.TextAlignment = TextAlignment.Left;
            
            documentReader.Document.Blocks.Add(t1a);
            documentReader.Document.Blocks.Add(ta);

            documentReader.Document.Blocks.Add(t2a);
            documentReader.Document.Blocks.Add(tb);
            documentReader.Document.Blocks.Add(tc);
            documentReader.Document.Blocks.Add(td);
            documentReader.Document.Blocks.Add(te);

            documentReader.Document.Blocks.Add(t2b);
            documentReader.Document.Blocks.Add(tf);

            documentReader.Document.Blocks.Add(LinkABibliaFalada);
            
        }

        private void HandleRequestNavigate(object sender, RoutedEventArgs e)
        {
            string navigateUri = (sender as Hyperlink).NavigateUri.ToString();
            this.presenter.OpenSite(navigateUri);
            e.Handled = true;
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

        private void AjudaMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.presenter.ShowHelp();
        }

        private void ABibliaFaladaMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.presenter.OpenSite(this.presenter.LinkSite);
        }

    }
}
