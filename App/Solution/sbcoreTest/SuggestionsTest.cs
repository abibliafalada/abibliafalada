using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sbcore.Components;
using sbcore.Persistence;
using sbcore.Model;
using sbcore.Model.Interface;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Collections;

namespace sbcoreTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SuggestionsTest
    {
        public SuggestionsTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;
        private static IObjectContainer container;
        private static SimpleTextSuggester suggester;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            container = Container.GetContainer(@"../../../SpokenBible" + SpokenBible.Properties.Resources.databaseFile);
            IEnumerable<Livro> livros = from Livro l in container
                                        select l;
            suggester = new SimpleTextSuggester(livros, ActivateSbItem);
        }
        
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Utitlities
        private static void ActivateSbItem(ISbItem item)
        {
            container.Activate(item, 5);
        }
        #endregion

        /// <summary>
        /// Teste para busca de livros específicos, nome ou sigla
        /// </summary>
        [TestMethod]
        public void BuscaLivro()
        {
            Dictionary<string, string> itens = new Dictionary<string,string>();
            itens.Add("Marcos", "Marcos");
            itens.Add("mc", "Marcos");
            itens.Add("João", "João");
            itens.Add("JS", "Josué");
            itens.Add("JO", "João");
            itens.Add("jó", "Jó");
            itens.Add("2 Crônicas", "2 Crônicas");
            itens.Add("2Cr", "2 Crônicas");

            suggester.GetSuggestionsFor("teste");

            Assert.IsInstanceOfType(suggester, typeof(SimpleTextSuggester), "TextSugester não inicializado.");
            Dictionary<string, string>.Enumerator enumerator = itens.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string resultado = suggester.GetBetterFor(enumerator.Current.Key);
                Assert.AreEqual(enumerator.Current.Value + ", ", resultado, "Busca por livro específico não encontrou o nome de Livro esperado.");
            }
        }
    }
}
