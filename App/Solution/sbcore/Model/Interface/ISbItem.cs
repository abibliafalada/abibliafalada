using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace sbcore.Model.Interface
{
    public interface ISbItem
    {
        string Display { get; }
        IEnumerable<ISbItem> Children { get; }
    }
}
