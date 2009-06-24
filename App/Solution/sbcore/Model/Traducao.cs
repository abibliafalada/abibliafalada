using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    class Traducao
    {
        public string Idioma { get; set; }
        public string Nome { get; set; }
        public IList<Testamento> Testamentos { get; set; }
    }
}
