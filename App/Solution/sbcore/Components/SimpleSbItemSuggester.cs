using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model.Interface;
using sbcore.Model;
using sbcore.Components.Interface;

namespace sbcore.Components
{
    public class SimpleSbItemSuggester : SimpleSuggester<ISbItem>
    {
        public SimpleSbItemSuggester(IEnumerable<Livro> itens, SbItemChildrenNeeded SbItemChildrenNeeded)
            : base(itens, SbItemChildrenNeeded)
        {
        }

        public override IEnumerable<ISbItem> GetSuggestionsFor(string term)
        {
            return MakeSuggestionsFor(term);
        }
    }
}
