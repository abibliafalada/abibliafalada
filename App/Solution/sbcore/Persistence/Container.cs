using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.TA;

namespace sbcore.Persistence
{
    public class Container
    {
        private static string file = @"..\..\..\..\Files\SbDbs\database.yap";

        private static IObjectContainer container = null;

        private Container() { }

        public static IObjectContainer GetContainer()
        {
            if (container == null)
            {
                //Db4oFactory.Configure().Add(new TransparentActivationSupport());
                container = Db4oFactory.OpenFile(file);
            }
            return container;
        }

        public static void CloseContainer()
        {
            container.Close();
        }
    }
}
