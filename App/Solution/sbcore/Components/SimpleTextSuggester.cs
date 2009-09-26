using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model;
using sbcore.Components.Interface;
using sbcore.Model.Interface;

namespace sbcore.Components
{
    public class SimpleTextSuggester : SimpleSuggester<string>
    {
        public SimpleTextSuggester(IEnumerable<Livro> itens, SbItemChildrenNeeded SbItemChildrenNeeded)
            : base(itens, SbItemChildrenNeeded)
        {
        }

        public override IEnumerable<string> GetSuggestionsFor(string term)
        {
            IList<string> itens = new List<string>();
            IEnumerable<ISbItem> allItens = MakeSuggestionsFor(term);

            if (allItens.Count() <= 0)
                return itens;

            if (allItens.Count() > 1 || allItens.First() is SbItemPair)
            {
                foreach (ISbItem item in allItens)
                {
                    itens.Add(SbItemToString(item));
                }
                return itens;
            }

            foreach (ISbItem item in allItens.First().Children)
            {
                itens.Add(SbItemToString(item));
            }
            return itens;
        }

        private string SbItemToString(ISbItem item)
        {
            if (item is Livro)
                return (item as Livro).Nome + ", ";
            if (item is Capitulo)
                return SbItemToString(item.Parent) + (item as Capitulo).Numero + ":";
            if (item is Versiculo)
                return SbItemToString(item.Parent) + (item as Versiculo).Numero;
            if (item is SbItemPair)
                return SbItemToString((item as SbItemPair).Item1) + "-" + ((item as SbItemPair).Item2 as Versiculo).Numero;
            return string.Empty;
        }
    }
}
