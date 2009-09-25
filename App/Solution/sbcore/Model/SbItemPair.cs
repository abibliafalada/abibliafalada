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
            get { return "pair"; }
        }

        public IEnumerable<ISbItem> Children
        {
            get { throw new NotImplementedException(); }
        }

        public ISbItem Parent
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
