using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace txt2sbdb
{
    class Program
    {
        static void Main(string[] args)
        {
            string arquivo = @"..\..\..\..\Files\Translations\pt_BR\ra.txt";

            Console.WriteLine("--- SpokenBible: file.txt to SpokenBible DataBase file converter ---");

            StreamReader sr = new StreamReader(arquivo);
            TxtParser parser = new TxtParser();

            string linha;
            while ((linha = sr.ReadLine()) != string.Empty)
            {
                parser.parse(linha);
            }

            sr.Close();
        }
    }
}
