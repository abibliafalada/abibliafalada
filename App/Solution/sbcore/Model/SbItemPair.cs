using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model.Interface;

namespace sbcore.Components
{
    public class SbItemPair : ISbItem
    {
        public ISbItem Item1 { get; set; }
        public ISbItem Item2 { get; set; }

        public SbItemPair(ISbItem item1, ISbItem item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        #region ISbItem Members

        public string Tag { get; set; }

        public string Display
        {
            get { return Item1.GetType().ToString() + " de " + Item1.Display + "-" + Item1.Display; }
        }

        public IEnumerable<ISbItem> Children
        {
            get
            {
                bool started = false;
                IList<ISbItem> itens = new List<ISbItem>();
                IEnumerator<ISbItem> allItens = Parent.Children.GetEnumerator();
                
                while(allItens.MoveNext())
                {
                    if (allItens.Current.Equals(Item1))
                        started = true;
                    if (started)
                        itens.Add(allItens.Current);
                    if (allItens.Current.Equals(Item2))
                        break;
                }

                return itens;
            }
        }

        public ISbItem Parent
        {
            get { return Item1.Parent; }
        }

        #endregion
    }
}
