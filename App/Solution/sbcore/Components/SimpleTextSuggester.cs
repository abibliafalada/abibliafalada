using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model;
using sbcore.Components.Interface;

namespace sbcore.Components
{
    public class SimpleTextSuggester : SimpleSuggester<string>
    {
        public SimpleTextSuggester(IEnumerable<Livro> itens, SbItemChildrenNeeded SbItemChildrenNeeded)
            : base(itens, SbItemChildrenNeeded)
        {
        }

        protected override string GetItem(Livro livro)
        {
            return livro.Nome + ", ";
        }

        protected override string GetItem(Livro livro, int cap)
        {
            return livro.Nome + ", " + cap;
        }

        protected override string GetItem(Livro livro, int cap, int vers1, int? vers2)
        {
            if (vers2 == null)
                return livro.Nome + ", " + cap + "." + vers1;
            else
                return livro.Nome + ", " + cap + "." + vers1 + "-" + vers2;
        }
    }
}
