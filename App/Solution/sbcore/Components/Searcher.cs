using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using sbcore.Persistence;
using sbcore.Components.Interface;
using sbcore.Model;
using sbcore.Model.Interface;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace sbcore.Components
{
    public class Searcher
    {
        #region Attributes and Properties
        private int maxOpenSearchResults = 100;
        public int MaxOpenSearchResults
        {
            get { return maxOpenSearchResults; }
            set { maxOpenSearchResults = value; }
        }

        private IObjectContainer DefaultContainer { get; set; }
        private Index DefaultIndex { get; set; }
        private ISuggestComponent<ISbItem> sbItemSuggest = null;
        #endregion

        #region Constructor
        public Searcher(string containerFileName, string indexFileName)
        {
            this.DefaultContainer = Container.GetContainer(containerFileName);
            this.DefaultIndex = new Index(indexFileName);

            IEnumerable<Livro> livros = from Livro l in DefaultContainer
                                        select l;
            this.sbItemSuggest = new SimpleSbItemSuggester(livros, ActivateSbItem);
        }
        #endregion


        #region Search processing
        public SbResultset Search(string term)
        {
            if (this.sbItemSuggest.GetSuggestionsFor(term).Count() > 0)
            {
                ISbItem opcao = this.sbItemSuggest.GetSuggestionsFor(term).First();
                return new SbResultset(opcao, SbResultsetType.Referencia);
            }

            try
            {
                int total = 0;
                IList<ISbItem> versiculos = OpenSearchRequest(term, out total);
                if (versiculos.Count > 0)
                {
                    SbResultset resultset = new SbResultset(versiculos, SbResultsetType.BuscaLivre);
                    resultset.TotalSearchResults = total;
                    return resultset;
                }
            }
            catch { }

            return null;
        }

        internal IList<ISbItem> OpenSearchRequest(string phrase, out int total)
        {
            IList<ISbItem> versiculos = new List<ISbItem>();

            IndexSearcher searcher = this.DefaultIndex.GetIndex();
            QueryParser queryParser = new QueryParser("versiculo", new StandardAnalyzer());
            Hits hits = searcher.Search(queryParser.Parse(phrase));
            total = hits.Length();
            for (int i = 0; i < (hits.Length() > maxOpenSearchResults ? maxOpenSearchResults : hits.Length()); i++)
            {
                ISbItem item = this.DefaultContainer.Ext().GetByID(Convert.ToInt64(hits.Doc(i).Get("id"))) as ISbItem;
                item.Tag = hits.Score(i).ToString();
                this.DefaultContainer.Activate(item, 1);
                versiculos.Add(item);
            }

            return versiculos;
        }
        #endregion

        #region General functions
        public void ActivateSbItem(ISbItem item)
        {
            this.DefaultContainer.Activate(item, 5);
        }
        #endregion
    }
}
