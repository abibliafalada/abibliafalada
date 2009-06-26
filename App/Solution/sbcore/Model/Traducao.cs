using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace sbcore.Model
{
    public class Traducao
    {
        #region Atributos e propriedades
        public IList<Testamento> testamentos;

        public string Idioma { get; set; }
        public string Nome { get; set; }

        public ReadOnlyCollection<Testamento> Testamentos
        {
            get
            {
                return new ReadOnlyCollection<Testamento>(testamentos);
            }
        }
        #endregion

        #region Construtores
        public Traducao(string idioma, string nome)
        {
            Idioma = idioma;
            Nome = nome;
            testamentos = new List<Testamento>();
        }
        #endregion

        #region Métodos
        public Testamento AddTestamento(Testamento testamento)
        {
            testamento.Traducao = this;
            this.testamentos.Add(testamento);
            return testamento;
        }
        #endregion
    }
}
