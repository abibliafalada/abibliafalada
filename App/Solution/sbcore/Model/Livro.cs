using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    public class Livro
    {
        #region Atributos e propriedades
        private IList<Capitulo> capitulos;

        public int Numero { get; set; }
        public string Acronimo { get; set; }
        public string Nome { get; set; }
        public Testamento Testamento { get; set; }

        public IList<Capitulo> Capitulos
        {
            get
            {
                if (capitulos == null)
                    capitulos = new List<Capitulo>();
                return capitulos;
            }
        }
        #endregion

        #region Construtor
        public Livro(int numero, string acronimo, string nome)
        {
            Numero = numero;
            Acronimo = acronimo;
            Nome = nome;
        }
        #endregion

        #region Métodos
        public Capitulo AddCapitulo(Capitulo capitulo)
        {
            capitulo.Livro = this;
            this.Capitulos.Add(capitulo);
            return capitulo;
        }
        #endregion
    }
}
