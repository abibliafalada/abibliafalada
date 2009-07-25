using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model;
using sbcore.Model.Interface;

namespace sbcore.Components.Interface
{
    public interface ISuggestComponent
    {
        IEnumerable<string> GetSuggestionsFor(string item);
        ISbItem GetOptionsFor(string item);
    }
}
