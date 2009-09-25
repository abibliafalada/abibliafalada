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
            foreach (ISbItem item in MakeSuggestionsFor(term))
            {
                itens.Add(item.Display);
            }
            return itens;
        }
    }
}
