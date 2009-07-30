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

        protected override ISbItem GetItem(Livro livro)
        {
            return livro;
        }

        protected override ISbItem GetItem(Livro livro, int cap)
        {
            return livro.Capitulos[cap - 1];
        }

        protected override ISbItem GetItem(Livro livro, int cap, int vers)
        {
            return livro.Capitulos[cap - 1].Versiculos[vers - 1];
        }
    }
}
