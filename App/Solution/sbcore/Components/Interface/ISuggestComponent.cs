using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model;
using sbcore.Model.Interface;

namespace sbcore.Components.Interface
{
    public delegate void SbItemChildrenNeeded(ISbItem item);

    public interface ISuggestComponent<T>
    {
        SbItemChildrenNeeded OnSbItemChildrenNeeded { get; set; }
        IEnumerable<T> GetSuggestionsFor(string item);
        T GetBetterFor(string item);
    }
}
