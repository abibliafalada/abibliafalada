using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Components.Interface;
using sbcore.Model;
using System.Text.RegularExpressions;
using sbcore.Model.Interface;

namespace sbcore.Components
{
    public abstract class SimpleSuggester<T> : ISuggestComponent<T>
    {

        public SbItemChildrenNeeded OnSbItemChildrenNeeded { get; set; }

        private IEnumerable<Livro> itens = null;

        private static string alfabetoAcentuado = @"a-zA-ZÀ-ÿ";
        //private static Regex versPattern = new Regex(@"^([0-3]?[" + alfabetoAcentuado + @"\ ]*)[\,\;\ ]*([0-9]*)[\:\.\,\ ]*([0-9\ ]*)([\-]([0-9\ ]*))?$", RegexOptions.IgnoreCase);
        private static Regex versPattern = new Regex(@"^([0-3]?[" + alfabetoAcentuado + @"\ ]*)([\,\;\ ]*)([0-9]*)([\:\.\,\ ]*)([0-9\ ]*)(\-?)([0-9\ ]*)$", RegexOptions.IgnoreCase);

        public SimpleSuggester(IEnumerable<Livro> itens, SbItemChildrenNeeded SbItemChildrenNeeded)
        {
            this.itens = itens;
            this.OnSbItemChildrenNeeded = SbItemChildrenNeeded;
        }

        #region ISuggestComponent Members
        public abstract IEnumerable<T> GetSuggestionsFor(string item);

        public T GetBetterFor(string term)
        {
            IEnumerable<T> suggestions = GetSuggestionsFor(term);
            return suggestions.First<T>();
        }
        #endregion

        public virtual IEnumerable<ISbItem> MakeSuggestionsFor(string term)
        {
            if (term == null)
                return null;

            term = term.Trim();

            Match m = versPattern.Match(term);

            string book = m.Groups[1].Value.Trim();
            string bookS = m.Groups[2].Value.Trim();
            string chap = m.Groups[3].Value.Trim();
            string chapS = m.Groups[4].Value.Trim();
            string vers1 = m.Groups[5].Value.Trim();
            string versS = m.Groups[6].Value.Trim();
            string vers2 = m.Groups[7].Value.Trim();

            return this.MakeSuggestionsFor(book, bookS, chap, chapS, vers1, versS, vers2);
        }

        public IEnumerable<ISbItem> MakeSuggestionsFor(string book, string bookS, string chap, string chapS, string vers1, string versS, string vers2)
        {
            //inicializacao
            IEnumerable<ISbItem> suggestions = new List<ISbItem>(0);

            //nenhuma busca
            if (book == string.Empty)
                return suggestions;

            //busca por livro
            suggestions = this.GetSuggestionsForBooks(book);

            if (bookS == string.Empty && chap == string.Empty)
                return suggestions;

            //busca por capitulo, depende do livro escolhido
            Livro livro = suggestions.First() as Livro;

            suggestions = this.GetSuggestionsForChapters(livro, chap);

            if (chapS == string.Empty && vers1 == string.Empty)
                return suggestions;

            //busca por versiculo, depende do capitulo especifico
            Capitulo capitulo = suggestions.First() as Capitulo;

            if (versS == string.Empty && vers2 == string.Empty)
            {
                suggestions = this.GetSuggestionsForVersicles(capitulo, vers1);
            }
            else
            {
                suggestions = this.GetSuggestionsForVersicles(capitulo, vers1, vers2);
            }

            return suggestions;
        }

        public IEnumerable<ISbItem> GetSuggestionsForBooks(string book)
        {
            IList<ISbItem> suggestions = new List<ISbItem>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Contains(book))
                    suggestions.Add(livro);
            }
            return suggestions;
        }

        public IEnumerable<ISbItem> GetSuggestionsForChapters(Livro livro, string chap)
        {
            IList<ISbItem> suggestions = new List<ISbItem>();
            for (int i = 1; i <= livro.Capitulos.Count; i++)
            {
                if (i.ToString().Contains(chap))
                {
                    suggestions.Add(livro.Capitulos[i-1]);
                }
            }
            return suggestions;
        }

        public IEnumerable<ISbItem> GetSuggestionsForVersicles(Capitulo capitulo, string vers)
        {
            IList<ISbItem> suggestions = new List<ISbItem>();
            OnSbItemChildrenNeeded(capitulo);

            for (int j = 1; j <= capitulo.Versiculos.Count; j++)
            {
                if (j.ToString().Contains(vers))
                {
                    suggestions.Add(capitulo.Versiculos[j-1]);
                }
            }
            
            return suggestions;
        }

        public IEnumerable<ISbItem> GetSuggestionsForVersicles(Capitulo capitulo, string vers1, string vers2)
        {
            IList<ISbItem> suggestions = new List<ISbItem>();
            OnSbItemChildrenNeeded(capitulo);

            int k = 0;
            int.TryParse(vers1, out k);

            for (int j = k; j <= capitulo.Versiculos.Count; j++)
            {
                if (j.ToString().Contains(vers2))
                {
                    suggestions.Add(new SbItemPair(capitulo.Versiculos[k-1], capitulo.Versiculos[j-1]));
                }
            }

            return suggestions;
        }

    }
}
