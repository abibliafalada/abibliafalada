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
using SpokenBible.Helpers;
using sbcore.Components;

namespace SpokenBible.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        #region Attributes and Methods
        MainPresenter presenter = null;
        #endregion

        #region Initializations
        public Main(MainPresenter presenter)
        {
            this.presenter = presenter;
            InitializeComponent();
            this.FecharBusca();
        }
        #endregion

        #region General functions
        public void ClearContent()
        {
            documentReader.Document.Blocks.Clear();
            FlowDocument trickToMoveToBegin = documentReader.Document;
            documentReader.Document = null;
            documentReader.Document = trickToMoveToBegin;
        }
        #endregion

        #region Help processing
        public void ShowHelp()
        {
            this.ShowHelp(false);
        }

        public void ShowHelp(bool exibirMensagemNaoEncontrado)
        {
            ClearContent();
            documentReader.Document.TextAlignment = TextAlignment.Left;
            documentReader.Document.Blocks.AddRange(StaticContentGenerator.GenerateHelpMessage(exibirMensagemNaoEncontrado));
        }
        #endregion

        #region Content processing
        public void ShowContent(SbResultset results)
        {
            ResultsetContentGenerator generator = new ResultsetContentGenerator();

            ClearContent();

            generator.StyleTitle = document.FindResource("StyleTitle") as Style;
            documentReader.Document.Blocks.AddRange(generator.GenerateParagraphs(results));
        }
        /*        
        private void CreateParagraph(ISbItem item)
        {
            Paragraph p = new Paragraph();
            p.Cursor = Cursors.Hand;
            p.Inlines.Add(item.Display);
            FormatParagraph(p, item);
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
        }*/
        #endregion

        #region Busca - temp
        private void AbrirBusca()
        {
            this.Conteudo.IsEnabled = false;
            this.BuscaBox.Visibility = Visibility.Visible;
        }

        private void FecharBusca()
        {
            this.Conteudo.IsEnabled = true;
            this.BuscaBox.Visibility = Visibility.Hidden;
        }
        #endregion

        #region GUI event processing
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
            StaticContentGenerator.OpenSite(SpokenBible.Properties.Resources.linkSite);
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                this.presenter.FullScreen = !this.presenter.FullScreen;
            }
        }

        private void buscar_Click(object sender, RoutedEventArgs e)
        {
            this.AbrirBusca();
        }

        private void SearchBox_BuscarButtonClick(object sender, RoutedEventArgs e)
        {
            this.presenter.BuscaRequested(this.BuscaBox.texto.Text);
            this.FecharBusca();
        }

        private void SearchBox_FecharButtonClick(object sender, RoutedEventArgs e)
        {
            this.FecharBusca();
        }
        #endregion
    }
}
