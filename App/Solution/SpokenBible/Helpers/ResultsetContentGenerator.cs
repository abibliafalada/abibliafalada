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
                        Paragraph t = new Paragraph();
                        t.Style = StyleTitle;
                        t.Inlines.Add(new Run(GetTitle(item)));
                        lastTitle = GetTitle(item);
                        blocks.Add(t);
                    }
                    Paragraph p = new Paragraph();
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
                Paragraph p = new Paragraph();
                //blocks.Add(NewParagraph(GetTitle(versiculo) + versiculo.Numero + versiculo.Descricao));
                p.Inlines.Add(new Bold(new Run(GetTitle(versiculo) + versiculo.Numero + ": ")));
                p.Inlines.Add(new Run(versiculo.Descricao));
                blocks.Add(p);
            }
            return blocks;
        }

        private Paragraph NewParagraph(string content)
        {
            Paragraph p = new Paragraph();
            p.Cursor = System.Windows.Input.Cursors.Hand;
            p.Inlines.Add(content);
            return p;
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
