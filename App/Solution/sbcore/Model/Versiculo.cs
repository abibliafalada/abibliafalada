using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model.Interface;
using System.Collections.ObjectModel;

namespace sbcore.Model
{
    public class Versiculo : ISbItem
    {
        #region Atributos e propriedades
        public string Tag { get; set; }
        public int Numero { get; set; }
        public string Descricao { get; set; }
        public Capitulo Capitulo;
        #endregion

        #region Construtor
        public Versiculo(int numero, string descricao)
        {
            Numero = numero;
            Descricao = descricao;
        }

        public ISbItem Parent
        {
            get { return Capitulo; }
        }

        #endregion

        #region ISbItem<Livro> Members

        public string Display
        {
            get { return Numero + ". " + Descricao; }
        }

        public IEnumerable<ISbItem> Children
        {
            get { return new Collection<ISbItem>(); }
        }

        #endregion
    }
}
