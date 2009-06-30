using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Model;

namespace txt2sbdb
{
    class TxtParser
    {
        public delegate void TokenFound<T>(T token);

        public TokenFound<Traducao> OnTraducaoFound { get; set; }
        public TokenFound<Testamento> OnTestamentoFound { get; set; }
        public TokenFound<Livro> OnLivroFound { get; set; }
        public TokenFound<Capitulo> OnCapituloFound { get; set; }
        public TokenFound<Versiculo> OnVersiculoFound { get; set; }

        private string idioma = string.Empty;
        private int numeroLivro = 0;
        private int numeroVersiculo = 0;

        internal void parse(string linha)
        {
            char[] ahifen = { '-' };
            string tipo;
            string conteudo;

            if (linha.IndexOf(':') > 0)
            {
                tipo = linha.Substring(0, linha.IndexOf(':')).Trim();
                conteudo = linha.Remove(0, linha.IndexOf(':') + 1).Trim();
                switch (tipo.ToLower())
                {
                    case "idioma":
                        idioma = conteudo;
                        break;
                    case "traducao":
                        OnTraducaoFound(new Traducao(idioma, conteudo));
                        break;
                    case "testamento":
                        string[] testamento = conteudo.Split(ahifen);
                        OnTestamentoFound(new Testamento(testamento[0].Trim(), testamento[1].Trim()));
                        break;
                    case "livro":
                        string[] livro = conteudo.Split(ahifen);
                        OnLivroFound(new Livro(++numeroLivro, livro[0].Trim(), livro[1].Trim()));
                        break;
                    case "capítulo":
                        numeroVersiculo = 0;
                        OnCapituloFound(new Capitulo(Convert.ToInt32(conteudo)));
                        break;
                    default:
                        string[] versiculo = linha.Split(ahifen, 2);
                        OnVersiculoFound(new Versiculo(++numeroVersiculo, versiculo[1].Trim()));
                        break;
                }
            }
            else
            {
                string[] versiculo = linha.Split(ahifen, 2);
                OnVersiculoFound(new Versiculo(++numeroVersiculo, versiculo[1].Trim()));
            }
        }
    }
}
