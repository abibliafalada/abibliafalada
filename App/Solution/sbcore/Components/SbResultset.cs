using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model.Interface;

namespace sbcore.Components
{
    public enum SbResultsetType
    {
        Referencia, BuscaLivre
    }

    public class SbResultset
    {
        public SbResultsetType Type { get; set; }
        public IEnumerable<ISbItem> Itens { get; set; }
        public int TotalSearchResults { get; set; }

        public SbResultset(ISbItem item, SbResultsetType type)
        {
            IList<ISbItem> itens = new List<ISbItem>();
            itens.Add(item);

            this.Type = type;
            this.Itens = itens;
        }

        public SbResultset(IEnumerable<ISbItem> itens, SbResultsetType type)
        {
            this.Type = type;
            this.Itens = itens;
        }
    }
}
