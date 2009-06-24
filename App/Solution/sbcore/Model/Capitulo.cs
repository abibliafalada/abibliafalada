using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    class Capitulo
    {
        public int Numero { get; set; }
        public IList<Versiculo> Versiculos { get; set; }
    }
}
