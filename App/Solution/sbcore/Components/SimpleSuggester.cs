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
        private static Regex bookPattern = new Regex(@"^([0-9]?[" + alfabetoAcentuado + @"\ ]+)$", RegexOptions.IgnoreCase);
        private static Regex chapPattern = new Regex(@"^([0-9]?[" + alfabetoAcentuado + @"\ ]+)[\,\;\-\ ]*([0-9]*)$", RegexOptions.IgnoreCase);
        private static Regex versPattern = new Regex(@"^([0-9]?[" + alfabetoAcentuado + @"\ ]+)[\,\;\-\ ]*([0-9]*)[\.\,\ ]*([0-9\ ]*)([\-]([0-9\ ]*))?$", RegexOptions.IgnoreCase);

        protected abstract T GetItem(Livro livro);
        protected abstract T GetItem(Livro livro, int cap);
        protected abstract T GetItem(Livro livro, int cap, int vers1, int? vers2);

        public SimpleSuggester(IEnumerable<Livro> itens, SbItemChildrenNeeded SbItemChildrenNeeded)
        {
            this.itens = itens;
            this.OnSbItemChildrenNeeded = SbItemChildrenNeeded;
        }

        #region ISuggestComponent Members
        public IEnumerable<T> GetSuggestionsFor(string term)
        {
            IEnumerable<T> resp = new List<T>(0);

            if (term == null)
                return resp;

            term = term.Trim();
            if (bookPattern.IsMatch(term))
                resp = this.GetSuggestionsForBooks(term);
            else if (chapPattern.IsMatch(term))
                resp = this.GetSuggestionsForChapters(term);
            else if (versPattern.IsMatch(term))
                resp = this.GetSuggestionsForVersicles(term);

            return resp.Take(20);
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
                        if (i.ToString().Contains(chap))
                            suggestions.Add(this.GetItem(livro, i));
                    break;
                }
            }
            return suggestions;
        }

        private IEnumerable<T> GetSuggestionsForVersicles(string term)
        {
            Match m = versPattern.Match(term);

            string book = m.Groups[1].Value.Trim();
            string chap = m.Groups[2].Value.Trim();
            string vers1 = m.Groups[3].Value.Trim();
            string vers2 = m.Groups[5].Value.Trim();

            IList<T> suggestions = new List<T>();
            foreach (Livro livro in this.itens)
            {
                if (livro.Contains(book))
                {
                    for (int i = 1; i <= livro.Capitulos.Count; i++)
                    {
                        if (i.ToString().Contains(chap))
                        {
                            OnSbItemChildrenNeeded(livro.Capitulos[i-1]);
                            if (vers2 == string.Empty)
                            {
                                for (int j = 1; j <= livro.Capitulos[i - 1].Versiculos.Count; j++)
                                {
                                    if (j.ToString().Contains(vers1))
                                    {
                                        suggestions.Add(this.GetItem(livro, i, j, null));
                                    }
                                }
                            }
                            else
                            {
                                int k = 0;
                                try
                                {
                                    k = Convert.ToInt32(vers1);
                                }
                                catch (Exception)
                                {
                                    return suggestions;
                                }

                                for (int j = k; j <= livro.Capitulos[i - 1].Versiculos.Count; j++)
                                {
                                    if (j.ToString().Contains(vers2))
                                    {
                                        suggestions.Add(this.GetItem(livro, i, k, j));
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
            return suggestions;
        }

    }
}
