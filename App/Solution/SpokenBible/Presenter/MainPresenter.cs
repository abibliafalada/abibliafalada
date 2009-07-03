using System;
using System.Linq;
using System.Text;
using System.Windows;
using SpokenBible.View;
using sbcore.Persistence;
using sbcore.Model;
using System.Collections;
using sbcore.Model.Interface;
using SpokenBible.Controller;
using System.Collections.Generic;
using Db4objects.Db4o.Linq;

namespace SpokenBible.Presenter
{
    public class MainPresenter
    {
        private MainWindow window = null;
        private AppController controller = null;

        public MainPresenter(AppController controller)
        {
            this.controller = controller;

            window = new MainWindow(this);

            IEnumerable<Livro> livros = from Livro l in controller.DefaultContainer
                                        select l;

            window.AutoCompleteItems = new List<Livro>(livros);

            ShowContent(livros.First<Livro>());

        }

        public void ShowView()
        {
            window.Show();
        }

        public void ShowContent(ISbItem item)
        {
            window.ShowContent(item);
        }

    }
}
