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
        private static SimpleSbItemSuggester suggester;

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
            container = Container.GetContainer(@"../../../SpokenBible" + SpokenBible.Components.SbDbManager.Database);
            IEnumerable<Livro> livros = from Livro l in container
                                        select l;
            suggester = new SimpleSbItemSuggester(livros, ActivateSbItem);
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
        /// Teste de sugestões de Livros
        /// </summary>
        [TestMethod]
        public void BuscaPorTermoDeveRetornarLivroCorreto()
        {
            Dictionary<string, ISbItem> itens = new Dictionary<string, ISbItem>();
            itens.Add("Marcos", new Livro(41, "Mc", "Marcos"));
            itens.Add("mc", new Livro(41, "Mc", "Marcos"));
            itens.Add("João", new Livro(43, "Jo", "João"));
            itens.Add("JS", new Livro(6, "Js", "Josué"));
            itens.Add("JO", new Livro(6, "Js", "Josué"));
            itens.Add("jó", new Livro(18, "Jó", "Jó"));
            itens.Add("2 Crônicas", new Livro(14, "IICr", "2 Crônicas"));
            itens.Add("IICr", new Livro(14, "IICr", "2 Crônicas"));

            Dictionary<string, ISbItem>.Enumerator enumerator = itens.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ISbItem resultado = suggester.GetBetterFor(enumerator.Current.Key);
                Assert.AreEqual(enumerator.Current.Value, resultado, "Sugestão não retornou o livro esperado.");
            }
        }

        /// <summary>
        /// Teste de sugestões de Capítulos
        /// </summary>
        [TestMethod]
        public void BuscaPorTermoDeveRetornarCapituloCorreto()
        {
            ISbItem resultado_Joao_3 = suggester.GetBetterFor("João 3");
            Assert.AreEqual(new Capitulo(3), resultado_Joao_3, "Sugestão não retornou o capítulo esperado.");
            Assert.AreEqual(new Livro(43, "Jo", "João"), resultado_Joao_3.Parent, "Sugestão não retornou o livro esperado.");
        }

        /// <summary>
        /// Teste de sugestões de Versículos
        /// </summary>
        [TestMethod]
        public void BuscaPorTermoDeveRetornarVersiculoCorreto()
        {
            ISbItem resultado_Joao_3_16 = suggester.GetBetterFor("João 3:16");
            Assert.AreEqual(new Versiculo(16, "Porque Deus amou ao mundo de tal maneira que deu o seu Filho unigênito, para que todo o que nele crê não pereça, mas tenha a vida eterna."),
                resultado_Joao_3_16, "Sugestão não retornou o versículo esperado.");
            Assert.AreEqual(new Capitulo(3), resultado_Joao_3_16.Parent, "Sugestão não retornou o capítulo esperado.");
            Assert.AreEqual(new Livro(43, "Jo", "João"), resultado_Joao_3_16.Parent.Parent, "Sugestão não retornou o livro esperado.");
        }
    }
}
