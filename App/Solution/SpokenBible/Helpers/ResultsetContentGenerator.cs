using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Components;
using System.Windows.Documents;
using sbcore.Model.Interface;
using sbcore.Model;
using System.Windows;
using System.Windows.Media;

namespace SpokenBible.Helpers
{
    public class ResultsetContentGenerator
    {
        public Style StyleTitle { get; set; }
        private SbResultset currentResultset = null;

        public System.Windows.Input.MouseButtonEventHandler OnParagraphMouseDown { get; set; }
        public System.Windows.Input.MouseButtonEventHandler OnReferenceMouseDown { get; set; }

        public Paragraph NewParagraph()
        {
            Paragraph p = new Paragraph();
            p.MouseDown += this.OnParagraphMouseDown;
            return p;
        }

        internal IList<Block> GenerateParagraphs(SbResultset resultset)
        {
            currentResultset = resultset;
            if(resultset.Type == SbResultsetType.Referencia)
                return GenerateParagraphsForReference(resultset.Itens);
            else
                return GenerateParagraphsForSearch(resultset.Itens);
        }

        private IList<Block> GenerateParagraphsForReference(IEnumerable<ISbItem> itens)
        {
            string lastTitle = string.Empty;
            List<Block> blocks = new List<Block>();
            foreach (ISbItem item in itens)
            {
                if (item is Versiculo)
                {
                    if (lastTitle != GetTitle(item))
                    {
                        Paragraph t = NewParagraph();
                        t.Style = StyleTitle;
                        t.Inlines.Add(new Run(GetTitle(item)));
                        lastTitle = GetTitle(item);
                        blocks.Add(t);
                    }
                    Paragraph p = NewParagraph();
                    p.Inlines.Add(new Run(item.Display));
                    blocks.Add(p);
                }
                else
                {
                    blocks.AddRange(GenerateParagraphsForReference(item.Children));
                }
            }
            return blocks;
        }

        private IList<Block> GenerateParagraphsForSearch(IEnumerable<ISbItem> itens)
        {
            IList<Block> blocks = new List<Block>();
            
            Paragraph pFound = NewParagraph();
            pFound.FontSize = 12;
            pFound.Padding = new Thickness(5);
            pFound.Background = Brushes.LightYellow;
            if (itens.Count() < currentResultset.TotalSearchResults)
                pFound.Inlines.Add(new Run("Exibindo os " + itens.Count() + " resultados mais relevantes de  um total de "));
            pFound.Inlines.Add(new Bold(new Run(currentResultset.TotalSearchResults.ToString() + " versículos encontrados.")));
            blocks.Add(pFound);
            
            foreach (Versiculo versiculo in itens)
            {
                Paragraph p = NewParagraph();
                Run referencia = new Run(GetTitle(versiculo) + versiculo.Numero + ":");
                referencia.TextDecorations = TextDecorations.Underline;
                referencia.Foreground = Brushes.SteelBlue;
                referencia.MouseDown += this.OnReferenceMouseDown;
                p.Inlines.Add(new Bold(referencia));
                p.Inlines.Add(new Run(" " + versiculo.Descricao));
                blocks.Add(p);
            }
            return blocks;
        }

        private string GetTitle(ISbItem item)
        {
            if (item == null)
                return string.Empty;

            if (item is Capitulo)
                return item.Parent.Display + " " + item.Display + ".";
            else
                return GetTitle(item.Parent);
        }

    }
}
