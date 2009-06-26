using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    public class Capitulo
    {
        #region Atributos e propriedades
        private IList<Versiculo> versiculos;

        public int Numero { get; set; }
        public Livro Livro { get; set; }

        public IList<Versiculo> Versiculos
        {
            get
            {
                if (versiculos == null)
                    versiculos = new List<Versiculo>();
                return versiculos;
            }
        }
        #endregion

        #region Construtor
        public Capitulo(int numero)
        {
            Numero = numero;
        }
        #endregion

        #region Métodos
        public Versiculo AddVersiculo(Versiculo versiculo)
        {
            versiculo.Capitulo = this;
            this.Versiculos.Add(versiculo);
            return versiculo;
        }
        #endregion
    }
}
