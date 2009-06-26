using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sbcore.Model
{
    public class Testamento
    {
        #region Atributos e propriedades
        private IList<Livro> livros;

        public string Acronimo { get; set; }
        public string Nome { get; set; }
        public Traducao Traducao { get; set; }

        public IList<Livro> Livros
        {
            get
            {
                if (livros == null)
                    livros = new List<Livro>();
                return livros;
            }
        }
        #endregion

        #region Construtor
        public Testamento(string acronimo, string nome)
        {
            Acronimo = acronimo;
            Nome = nome;
        }
        #endregion

        #region Métodos
        public Livro AddLivro(Livro livro)
        {
            livro.Testamento = this;
            this.Livros.Add(livro);
            return livro;
        }
        #endregion
    }
}
