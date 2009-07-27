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
        private IEnumerable<Livro> itens = null;

        private Regex bookPattern = new Regex("^([0-9a-zA-Z ]+)$");
        private Regex chapPattern = new Regex("^([0-9a-zA-Z ]+)[,;-]([0-9 ]*)$");
            
        public SimpleSuggester(IEnumerable<Livro> itens)
        {
            this.itens = itens;
        }

        #region ISuggestComponent Members
        public IEnumerable<T> GetSuggestionsFor(string term)
        {
            if (bookPattern.IsMatch(term))
                return this.GetSuggestionsForBooks(term);
            if (chapPattern.IsMatch(term))
                return this.GetSuggestionsForChapters(term);

            return new List<T>(0);
        }
        #endregion

        private IEnumerable<T> GetSuggestionsForBooks(string term)
        {
            string book = term.Trim();
            IList<T> suggestions = new List<T>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Contains(book))
                    suggestions.Add(this.GetItem(livro));
            }
            return suggestions;
        }

        protected abstract T GetItem(Livro livro);
        protected abstract T GetItem(Livro livro, int i);

        private IEnumerable<T> GetSuggestionsForChapters(string term)
        {
            Match m = chapPattern.Match(term);

            string book = m.Groups[1].Value.Trim();
            string chap = m.Groups[2].Value.Trim();

            IList<T> suggestions = new List<T>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Contains(book))
                {
                    for (int i = 1; i <= livro.Capitulos.Count; i++)
                        if(i.ToString().Contains(chap))
                            suggestions.Add(this.GetItem(livro, i));
                    break;
                }
            }
            return suggestions;
        }
    }
}
