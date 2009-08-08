using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using sbcore.Model.Interface;

namespace sbcore.Model
{
    public class Capitulo : ISbItem
    {
        #region Atributos e propriedades
        private IList<Versiculo> versiculos;

        public int Numero { get; set; }
        public Livro Livro { get; set; }

        public IList<Versiculo> Versiculos
        {
            get
            {
                return new ReadOnlyCollection<Versiculo>(versiculos);
            }
        }
        #endregion

        #region Construtor
        public Capitulo(int numero)
        {
            Numero = numero;
            versiculos = new List<Versiculo>();
        }
        #endregion

        #region Métodos
        public Versiculo AddVersiculo(Versiculo versiculo)
        {
            versiculo.Capitulo = this;
            this.versiculos.Add(versiculo);
            return versiculo;
        }
        #endregion

        #region ISbItem Members

        public string Display
        {
            get { return "Capítulo: " + Numero.ToString(); }
        }

        public IEnumerable<ISbItem> Children
        {
            get { return Enumerable.Cast<ISbItem>(Versiculos); }
        }

        #endregion
    }
}
