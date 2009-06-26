using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    public class Versiculo
    {
        #region Atributos e propriedades
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
        #endregion
    }
}
