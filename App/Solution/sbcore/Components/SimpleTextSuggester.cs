using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model;

namespace sbcore.Components
{
    public class SimpleTextSuggester : SimpleSuggester<string>
    {
        public SimpleTextSuggester (IEnumerable<Livro> itens) : base (itens)
        {
        }

        protected override string GetItem(Livro livro)
        {
            return livro.Nome + ", ";
        }

        protected override string GetItem(Livro livro, int i)
        {
            return livro.Nome + ", " + i;
        }
    }
}
