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
    public class SimpleSuggester : ISuggestComponent
    {
        private IEnumerable<Livro> itens = null;

        private Regex bookPattern = new Regex("^([0-9a-zA-Z ]+)$");
        private Regex chapPattern = new Regex("^([0-9a-zA-Z ]+)[,;-]([0-9 ]*)$");
            
        public SimpleSuggester(IEnumerable<Livro> itens)
        {
            this.itens = itens;
        }

        #region ISuggestComponent Members
        public IEnumerable<string> GetSuggestionsFor(string term)
        {
            if (bookPattern.IsMatch(term))
                return this.GetSuggestionsForBooks(term);
            if (chapPattern.IsMatch(term))
                return this.GetSuggestionsForChapters(term);

            IList<string> suggestions = new List<string>(0);
            return suggestions;
        }

        public ISbItem GetOptionsFor(string item)
        {
            //todo: refatorar buscas e completar este código
            return this.itens.First();
        }
        #endregion

        private IEnumerable<string> GetSuggestionsForBooks(string term)
        {
            string book = term.Trim();
            IList<string> suggestions = new List<string>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Nome.Contains(book))
                    suggestions.Add(livro.Nome + ", ");
            }
            return suggestions;
        }

        private IEnumerable<string> GetSuggestionsForChapters(string term)
        {
            Match m = chapPattern.Match(term);

            string book = m.Groups[1].Value.Trim();
            string chap = m.Groups[2].Value.Trim();

            IList<string> suggestions = new List<string>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Nome.Contains(book))
                {
                    for (int i = 1; i <= livro.Capitulos.Count; i++)
                        if(i.ToString().Contains(chap))
                            suggestions.Add(livro.Nome + ", " + i);
                    break;
                }
            }
            return suggestions;
        }
    }
}
