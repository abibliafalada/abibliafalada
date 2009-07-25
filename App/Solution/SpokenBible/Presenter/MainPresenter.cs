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
using sbcore.Components.Interface;
using sbcore.Components;
using Db4objects.Db4o;

namespace SpokenBible.Presenter
{
    public class MainPresenter
    {
        private MainWindow window = null;
        private AppController controller = null;
        private ISuggestComponent suggest = null;

        public MainPresenter(AppController controller)
        {
            this.controller = controller;
            this.window = new MainWindow(this);

            IEnumerable<Livro> livros = from Livro l in controller.DefaultContainer
                                        select l;

            this.suggest = new SimpleSuggester(livros);
        }

        public void ShowView()
        {
            window.Show();
        }

        public void ShowContent(ISbItem item)
        {
            window.ShowContent(item);
        }


        internal void SearchChanged(string term)
        {
            IEnumerable<string> sugestoes = this.suggest.GetSuggestionsFor(term);
            this.window.UpdateSuggestions(sugestoes);
        }

        internal void SearchRequested(string term)
        {
            ISbItem opcao = this.suggest.GetOptionsFor(term).Children.First();
            this.controller.DefaultContainer.Activate(opcao, 5);
            this.ShowContent(opcao);
        }
    }
}
