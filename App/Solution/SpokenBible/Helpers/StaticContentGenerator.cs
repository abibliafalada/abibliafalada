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
        public static System.Windows.Input.MouseButtonEventHandler OnParagraphMouseDown { get; set; }

        public static Paragraph NewParagraph()
        {
            Paragraph p = new Paragraph();
            p.MouseDown += OnParagraphMouseDown;
            return p;
        }

        public static Paragraph LinkABibliaFalada
        {
            get
            {
                Paragraph paragraphABibliaFalada = NewParagraph();
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

            Paragraph ne = NewParagraph();

            Paragraph t1a = NewParagraph();

            Paragraph t2a = NewParagraph();
            Paragraph t2b = NewParagraph();
            Paragraph t2c = NewParagraph();

            Paragraph ta = NewParagraph();
            Paragraph tb = NewParagraph();
            Paragraph tc = NewParagraph();
            Paragraph td = NewParagraph();
            Paragraph te = NewParagraph();
            Paragraph tf = NewParagraph();
            Paragraph tg = NewParagraph();
            Paragraph th = NewParagraph();
            Paragraph ti = NewParagraph();
            Paragraph tj = NewParagraph();
            Paragraph tk = NewParagraph();

            ne.FontSize = 16;
            ne.Margin = margemTitulo;
            ne.Foreground = Brushes.Red;

            t1a.FontSize = 26;
            t1a.Margin = margemTitulo;

            t2a.FontSize = 24;
            t2a.Margin = margemTitulo;
            t2b.FontSize = 24;
            t2b.Margin = margemTitulo;
            t2c.FontSize = 24;
            t2c.Margin = margemTitulo;

            ta.Margin = margemParagrafo;
            tb.Margin = margemParagrafo;
            tc.Margin = margemParagrafo;
            td.Margin = margemParagrafo;
            te.Margin = margemParagrafo;
            tf.Margin = margemParagrafo;
            tg.Margin = margemParagrafo;
            th.Margin = margemParagrafo;
            ti.Margin = margemParagrafo;
            tj.Margin = margemParagrafo;
            tk.Margin = margemParagrafo;

            ne.Inlines.Add("A Passagem informada não foi encontrada.");
            t1a.Inlines.Add("Ajuda da Bíblia Falada 2.0 – Gênesis");
            t2a.Inlines.Add("Como procurar por uma passagem específica?");
            t2b.Inlines.Add("Não consegue ouvir os textos em português?");
            t2c.Inlines.Add("Como fazer uma busca livre?");

            ta.Inlines.Add(new Run("Envie suas sugestões para: "));
            ta.Inlines.Add(new Run("sugestoes@abibliafalada.com.br"));

            tb.Inlines.Add(new Run("Além de selecionar um livro acessando o menu à esquerda, você também pode utilizar as formas mais comuns de referenciar os textos bíblicos para encontrar as passagens desejadas."));

            tc.Inlines.Add(new Run("Por exemplo, para encontrar o Salmo de número 23, basta digitar no campo de busca:"));
            tc.Inlines.Add(new LineBreak());
            tc.Inlines.Add(new Bold(new Run("Salmo, 23 ")));
            tc.Inlines.Add(new Run("- ou simplesmente: "));
            tc.Inlines.Add(new Bold(new Run("Sl23")));

            td.Inlines.Add(new Run("Também é possível buscar um livro completo: "));
            td.Inlines.Add(new Bold(new Run("João")));

            te.Inlines.Add(new Run("Ou mesmo alguns versículos de um capítulo específico: "));
            te.Inlines.Add(new Bold(new Run("João, 3:16-18")));

            tf.Inlines.Add(new Run("Para ouvir os textos em português é necessário instalar uma voz neste idioma:"));
            tf.Inlines.Add(new LineBreak());
            tf.Inlines.Add(linkRaquel);
            tf.Inlines.Add(new LineBreak());

            tg.Inlines.Add(new Run("Encontre o versículo que desejar! Você pode procurar qualquer palavra ou frase em toda a Bíblia Sagrada, de forma semelhante à que você faz no Google, por exemplo:"));
            th.Inlines.Add(new Bold(new Run("Jesus ")));
            th.Inlines.Add(new Run(" irá retornar todos os versículos em que a palavra \"Jesus\" aparece, por ordem de importância. Se você utilizar duas ou mais palavras, os resultados poderão trazer itens com apenas uma das palavras."));
            ti.Inlines.Add(new Run("Para restringir apenas aos versículos em que determinada palavra deva obrigatoriamente aparecer, utilize o símbolo + imediatamente antes desta palavra:"));
            ti.Inlines.Add(new LineBreak());
            ti.Inlines.Add(new Bold(new Run("+pai + nosso ")));
            ti.Inlines.Add(new Run(" enquanto a busca por \"pai nosso\" encontra 1354 versículos, a busca por \"+pai +nosso\" retorna apenas 66."));
            tj.Inlines.Add(new Run("Também pode-se restringir a pesquisa a uma frase exata, utilizando-se aspas. Seguindo o exemplo:"));
            tj.Inlines.Add(new LineBreak());
            tj.Inlines.Add(new Bold(new Run("\"pai nosso\" ")));
            tj.Inlines.Add(new Run(" retorna apenas 2 versículos."));
            tk.Inlines.Add(new Run("Outras dicas:"));
            tk.Inlines.Add(new LineBreak());
            tk.Inlines.Add(new Bold(new Run("+Lucas -Marcos ")));
            tk.Inlines.Add(new Run(" versículos em que a palavra \"Lucas\" aparece mas a palavra \"Marcos\" não."));
            tk.Inlines.Add(new LineBreak());
            tk.Inlines.Add(new Bold(new Run("luz* ")));
            tk.Inlines.Add(new Run(" versículos que contenham pelo menos uma palavra iniciada por \"luz\". Por exemplo: luzentes, luzeiros, luzia."));
            
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

            blocks.Add(t2c);
            blocks.Add(tg);
            blocks.Add(th);
            blocks.Add(ti);
            blocks.Add(tj);
            blocks.Add(tk);

            blocks.Add(t2b);
            blocks.Add(tf);

            blocks.Add(LinkABibliaFalada);
            return blocks;
        }
        #endregion
    }
}
