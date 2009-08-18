using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model.Interface;
using sbcore.Model;
using sbcore.Components.Interface;

namespace sbcore.Components
{
    public class SimpleSbItemSuggester : SimpleSuggester<IEnumerable<ISbItem>>
    {
        public SimpleSbItemSuggester(IEnumerable<Livro> itens, SbItemChildrenNeeded SbItemChildrenNeeded)
            : base(itens, SbItemChildrenNeeded)
        {
        }

        protected override IEnumerable<ISbItem> GetItem(Livro livro)
        {
            IList<ISbItem> resp = new List<ISbItem>();
            resp.Add(livro);
            return resp;
        }

        protected override IEnumerable<ISbItem> GetItem(Livro livro, int cap)
        {
            IList<ISbItem> resp = new List<ISbItem>();
            resp.Add(livro.Capitulos[cap - 1]);
            return resp;
        }

        protected override IEnumerable<ISbItem> GetItem(Livro livro, int cap, int vers1, int? vers2)
        {
            IList<ISbItem> resp = new List<ISbItem>();
            
            if(vers2 == null){
                resp.Add(livro.Capitulos[cap - 1].Versiculos[vers1 - 1]);
                return resp;
            }

            for (int i = vers1; i <= vers2; i++ )
                resp.Add(livro.Capitulos[cap - 1].Versiculos[i - 1]);
            return resp;
        }
    }
}
