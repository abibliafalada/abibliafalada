using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Components.Interface;
using sbcore.Model;
using System.Text.RegularExpressions;

namespace sbcore.Components
{
    public class SimpleSuggester : ISuggestComponent
    {
        private IEnumerable<Livro> itens = null;
        
        public SimpleSuggester(IEnumerable<Livro> itens)
        {
            this.itens = itens;
        }

        #region ISuggestComponent Members
        public IEnumerable<string> GetSuggestionsFor(string term)
        {
            Regex livroPattern = new Regex("^[0-9a-zA-Z ]+$");
            Regex capituloPattern = new Regex("^[0-9a-zA-Z ]+[,;-][0-9 ]*$");

            if (livroPattern.IsMatch(term))
                return this.GetSuggestionsForBooks(term);
            if (capituloPattern.IsMatch(term))
                return this.GetSuggestionsForChapters(term);

            IList<string> suggestions = new List<string>(1);
            suggestions.Add("Busca genérica");
            return suggestions;
        }
        #endregion

        private IEnumerable<string> GetSuggestionsForBooks(string term)
        {
            IList<string> suggestions = new List<string>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Nome.Contains(term))
                    suggestions.Add(livro.Nome + ", ");
            }
            return suggestions;
        }

        private IEnumerable<string> GetSuggestionsForChapters(string term)
        {
            Regex livroPattern = new Regex("^([0-9a-zA-Z ]+)[,;-]([0-9 ])*$");
            Match m = livroPattern.Match(term);

            string book = m.Groups[1].Value;
            string chap = m.Groups[2].Value;

            IList<string> suggestions = new List<string>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Nome.Contains(book))
                {
                    for (int i = 1; i <= livro.Capitulos.Count; i++)
                        suggestions.Add(livro.Nome + ", " + i);
                    break;
                }
            }
            return suggestions;
        }
    }
}
