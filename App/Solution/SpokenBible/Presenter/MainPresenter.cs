﻿using System;
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
        private ISuggestComponent<string> textSuggest = null;
        private ISuggestComponent<ISbItem> sbItemSuggest = null;

        public MainPresenter(AppController controller)
        {
            this.controller = controller;
            this.window = new MainWindow(this);

            IEnumerable<Livro> livros = from Livro l in controller.DefaultContainer
                                        select l;

            this.textSuggest = new SimpleTextSuggester(livros, ActivateSbItem);
            this.sbItemSuggest = new SimpleSbItemSuggester(livros, ActivateSbItem);
        }

        public void ShowView()
        {
            window.Show();
        }

        public void ShowContent(ISbItem item)
        {
            window.ClearContent();
            window.ShowContent(item);
        }


        internal void SearchChanged(string term)
        {
            IEnumerable<string> sugestoes = this.textSuggest.GetSuggestionsFor(term);
            this.window.UpdateSuggestions(sugestoes);
        }

        internal void ActivateSbItem(ISbItem item)
        {
            this.controller.DefaultContainer.Activate(item, 5);
        }

        internal void SearchRequested(string term)
        {
            if (this.sbItemSuggest.GetSuggestionsFor(term).Count() > 0)
            {
                ISbItem opcao = this.sbItemSuggest.GetSuggestionsFor(term).First();
                this.controller.DefaultContainer.Activate(opcao, 5);
                this.ShowContent(opcao);
            }
            else
            {
                ShowContent(new Livro(0, "Nao", "Não encontrado"));
            }
        }
    }
}
