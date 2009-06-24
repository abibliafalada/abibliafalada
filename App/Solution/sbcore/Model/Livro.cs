using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    class Livro
    {
        public int Numero { get; set; }
        public string Acronimo { get; set; }
        public string Nome { get; set; }
        public IList<Capitulo> Capitulos { get; set; }
    }
}
