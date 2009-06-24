using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace txt2sbdb
{
    class TxtParser
    {
        internal void parse(string linha)
        {
            string tipo;
            if (linha.IndexOf(':') > 0)
            {
                tipo = linha.Substring(0, linha.IndexOf(':'));
                switch (tipo.ToLower())
                {
                    case "idioma":
                        break;
                    case "traducao":
                        break;
                    case "livro":
                        break;
                    case "capitulo":
                        break;
                    default:
                        break;
                }
                Console.WriteLine(tipo);
            }
        }
    }
}
