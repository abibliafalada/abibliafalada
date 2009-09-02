using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using sbcore.Model;
using sbcore.Persistence;
using Db4objects.Db4o;

namespace txt2sbdb
{
    class Program
    {
        private Traducao currentTraducao = null;
        private Testamento currentTestamento = null;
        private Livro currentLivro = null;
        private Capitulo currentCapitulo = null;
        private Versiculo currentVersiulo = null;

        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            string arquivoIn = @"..\..\..\..\Files\Translations\pt_BR\ra.txt";
            string arquivoOut = @"..\..\..\SpokenBible\Data\database.yap";
            
            Console.WriteLine("--- SpokenBible: file.txt to SpokenBible DataBase file converter ---");

            File.Delete(arquivoOut);
            StreamReader sr = new StreamReader(arquivoIn);
            TxtParser parser = new TxtParser();
            parser.OnTraducaoFound = new TxtParser.TokenFound<Traducao>(this.traducaoFound);
            parser.OnTestamentoFound = new TxtParser.TokenFound<Testamento>(this.testamentoFound);
            parser.OnLivroFound = new TxtParser.TokenFound<Livro>(this.livroFound);
            parser.OnCapituloFound = new TxtParser.TokenFound<Capitulo>(this.capituloFound);
            parser.OnVersiculoFound = new TxtParser.TokenFound<Versiculo>(this.versiculoFound);

            string linha;
            while ((linha = sr.ReadLine()) != null)
            {
                parser.parse(linha);
            }

            sr.Close();

            Console.WriteLine("Txt to Object conversion finalized.");

            IObjectContainer container = Container.GetContainer(arquivoOut);
            container.Store(currentTraducao);
            Container.CloseContainer();

            Console.WriteLine("Database created.");

            Console.ReadKey();
        }

        private void traducaoFound(Traducao traducao)
        {
            currentTraducao = traducao;
        }

        private void testamentoFound(Testamento testamento)
        {
            currentTestamento = currentTraducao.AddTestamento(testamento);
        }

        private void livroFound(Livro livro)
        {
            currentLivro = currentTestamento.AddLivro(livro);
        }

        private void capituloFound(Capitulo capitulo)
        {
            currentCapitulo = currentLivro.AddCapitulo(capitulo);
        }

        private void versiculoFound(Versiculo versiculo)
        {
            currentVersiulo = currentCapitulo.AddVersiculo(versiculo);
        }
    }
}
