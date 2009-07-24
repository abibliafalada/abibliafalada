using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Config;

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
                //Db4oFactory.Configure().ActivationDepth(1);
                //Db4oFactory.Configure().ObjectClass(typeof(sbcore.Model.Livro)).CascadeOnActivate(false);
                Db4oFactory.Configure().ObjectClass(typeof(sbcore.Model.Livro)).MaximumActivationDepth(3);
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
