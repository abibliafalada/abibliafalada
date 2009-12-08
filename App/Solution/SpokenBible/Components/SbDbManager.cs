using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpokenBible.Properties;
using System.IO;
using sbcore.Model;
using sbcore.Persistence;
using Db4objects.Db4o;

namespace SpokenBible.Components
{
    public class SbDbManager
    {
        private static string defaultDir = @".\Data\";
        private static string defaultDbExt = @".yap";
        
        public static string Database
        {
            get
            {
                return defaultDir + Settings.Default.TraducaoAtual + defaultDbExt;
            }
        }

        public static string Index
        {
            get
            {
                return defaultDir + Settings.Default.TraducaoAtual;
            }
        }

        public IEnumerable<Traducao> Databases
        {
            get
            {
                IList<Traducao> traducoes = new List<Traducao>();
                IObjectContainer container = null;
                foreach (string database in Directory.GetFiles(defaultDir, "*"+defaultDbExt))
                {
                    container = Container.GetContainer(database);
                    foreach (Traducao traducao in container.Query<Traducao>())
                    {
                        traducoes.Add(traducao);
                    }
                }
                return traducoes;
            }
        }
    }
}
