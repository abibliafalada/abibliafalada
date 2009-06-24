using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    class Testamento
    {
        public string Acronimo { get; set; }
        public string Nome { get; set; }
        public IList<Livro> Livros { get; set; }

    }
}
