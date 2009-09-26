using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Components;
using System.Windows.Documents;
using sbcore.Model.Interface;
using sbcore.Model;
using System.Windows;

namespace SpokenBible.Helpers
{
    public class ResultsetContentGenerator
    {
        public Style StyleTitle { get; set; }

        public System.Windows.Input.MouseButtonEventHandler OnParagraphMouseDown { get; set; }

        public Paragraph NewParagraph()
        {
            Paragraph p = new Paragraph();
            p.MouseDown += this.OnParagraphMouseDown;
            return p;
        }

        internal IList<Block> GenerateParagraphs(SbResultset resultset)
        {
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
            foreach (Versiculo versiculo in itens)
            {
                Paragraph p = NewParagraph();
                p.Inlines.Add(new Run(versiculo.Tag + ": "));
                p.Inlines.Add(new Bold(new Run(GetTitle(versiculo) + versiculo.Numero + ": ")));
                p.Inlines.Add(new Run(versiculo.Descricao));
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
