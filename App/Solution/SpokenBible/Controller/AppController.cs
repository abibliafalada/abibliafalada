using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Persistence;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using SpokenBible.Presenter;
using sbcore.Model;

namespace SpokenBible.Controller
{
    public class AppController
    {
        IObjectContainer container = null;

        public void Start()
        {
            container = Container.GetContainer();
            IEnumerable<Livro> livros = from Livro l in container
                                  select l;

            MainPresenter presenter = new MainPresenter();
            presenter.ShowContent(livros.First<Livro>());
            presenter.ShowView();
        }

        internal void End()
        {
            Container.CloseContainer();
        }
    }
}