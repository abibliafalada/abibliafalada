using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model.Interface;
using sbcore.Model;

namespace sbcore.Components
{
    public class SimpleSbItemSuggester : SimpleSuggester<ISbItem>
    {
        public SimpleSbItemSuggester(IEnumerable<Livro> itens) : base(itens)
        {
        }

        protected override ISbItem GetItem(Livro livro)
        {
            return livro;
        }

        protected override ISbItem GetItem(Livro livro, int i)
        {
            return livro.Children.ElementAt(i-1);
        }
    }
}
