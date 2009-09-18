using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SpokenBible.Helpers
{
    class StaticContentGenerator
    {
        #region General purpouse
        public static Paragraph LinkABibliaFalada
        {
            get
            {
                Paragraph paragraphABibliaFalada = new Paragraph();
                paragraphABibliaFalada.TextAlignment = TextAlignment.Right;

                Hyperlink linkABibliaFalada = new Hyperlink(new Run("A Bíblia Falada"));
                linkABibliaFalada.RequestNavigate += HandleRequestNavigate;
                linkABibliaFalada.NavigateUri = new Uri(SpokenBible.Properties.Resources.linkSite);

                paragraphABibliaFalada.Inlines.Add(linkABibliaFalada);

                return paragraphABibliaFalada;
            }
        }

        private static void HandleRequestNavigate(object sender, RoutedEventArgs e)
        {
            string navigateUri = (sender as Hyperlink).NavigateUri.ToString();
            OpenSite(navigateUri);
            e.Handled = true;
        }

        internal static void OpenSite(string uri)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(uri));
        }
        #endregion

        #region Messages
        internal static IList<Block> GenerateHelpMessage(bool exibirMensagemNaoEncontrado)
        {
            Thickness margemTitulo = new Thickness(5, 20, 5, 5);
            Thickness margemParagrafo = new Thickness(5);

            Hyperlink linkRaquel = new Hyperlink(new Run("Voz em Português"));
            linkRaquel.RequestNavigate += HandleRequestNavigate;
            linkRaquel.NavigateUri = new Uri(SpokenBible.Properties.Resources.linkVozPortugues);

            Paragraph ne = new Paragraph();

            Paragraph t1a = new Paragraph();

            Paragraph t2a = new Paragraph();
            Paragraph t2b = new Paragraph();

            Paragraph ta = new Paragraph();
            Paragraph tb = new Paragraph();
            Paragraph tc = new Paragraph();
            Paragraph td = new Paragraph();
            Paragraph te = new Paragraph();
            Paragraph tf = new Paragraph();

            ne.FontSize = 16;
            ne.Margin = margemTitulo;
            ne.Foreground = Brushes.Red;

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

            ne.Inlines.Add("A Passagem informada não foi encontrada.");
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

            tf.Inlines.Add(new Run("Para ouvir os textos em português é necessário instalar uma voz neste idioma:"));
            tf.Inlines.Add(new LineBreak());
            tf.Inlines.Add(linkRaquel);
            tf.Inlines.Add(new LineBreak());

            IList<Block> blocks = new List<Block>();

            if (exibirMensagemNaoEncontrado)
                blocks.Add(ne);

            blocks.Add(t1a);
            blocks.Add(ta);

            blocks.Add(t2a);
            blocks.Add(tb);
            blocks.Add(tc);
            blocks.Add(td);
            blocks.Add(te);

            blocks.Add(t2b);
            blocks.Add(tf);

            blocks.Add(LinkABibliaFalada);
            return blocks;
        }
        #endregion
    }
}
