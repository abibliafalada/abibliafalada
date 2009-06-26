using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;

namespace sbcore.Persistence
{
    public class Container
    {
        private static string file = @"database.yap";

        private static IObjectContainer container = null;

        private Container() { }

        public static IObjectContainer GetContainer()
        {
            if(container == null)
                container = Db4oFactory.OpenFile(file);
            return container;
        }

        public static void CloseContainer()
        {
            container.Close();
        }
    }
}
