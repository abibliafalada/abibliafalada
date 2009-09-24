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
using sbcore.Components.Interface;
using sbcore.Components;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Speech.Synthesis;
using System.Windows.Controls;
using AltzControls;
using System.Windows.Automation;
using System.Windows.Input;
using System.Globalization;
using Lucene.Net.Search;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Documents;

namespace SpokenBible.Presenter
{
    public class MainPresenter
    {
        #region Attributes and Methods
        private MainWindow mainWindow = null;
        private Main mainPage = null;
        private Principal principalPage = null;
        private Shortcuts shortcutsPage = null;
        private AppController controller = null;
        
        private ISuggestComponent<string> textSuggest = null;
        private ISuggestComponent<IEnumerable<ISbItem>> sbItemSuggest = null;

        private SpeechSynthesizer synthetizer = null;

        private WindowState lastWindowState;
        private WindowStyle lastWindowStyle;
        private bool fullScreen = false;
        #endregion

        #region Initializations
        public MainPresenter(AppController controller)
        {
            //inicialização dos controles
            this.controller = controller;
            this.mainWindow = new MainWindow(this);
            this.mainPage = new Main(this);
            this.principalPage = new Principal(this);
            this.shortcutsPage = new Shortcuts(this);

            //carregamento de paginas e conteudos visuais adicionais
            this.mainWindow.principal.Navigate(this.principalPage);
            this.mainWindow.shortcuts.Navigate(this.shortcutsPage);

            //carregamento dos dados
            IEnumerable < Livro > livros = from Livro l in controller.DefaultContainer
                                           select l;

            this.textSuggest = new SimpleTextSuggester(livros, ActivateSbItem);
            this.sbItemSuggest = new SimpleSbItemSuggester(livros, ActivateSbItem);

            this.shortcutsPage.VelhoTestamento = from Livro l in livros
                                                 where l.Testamento.Acronimo == "AT"
                                                 select l;
            this.shortcutsPage.NovoTestamento = from Livro l in livros
                                                 where l.Testamento.Acronimo == "NT"
                                                 select l;

            //carregamento das configurações salvas
            this.principalPage.busca.Text = this.controller.DefaultTerm;
        }

        public void ShowView()
        {
            mainWindow.Show();
        }
        #endregion

        #region Voice synthesizer
        private SpeechSynthesizer Synthetizer
        {
            get
            {
                if (synthetizer == null)
                    synthetizer = new SpeechSynthesizer();
                return synthetizer;
            }
        }

        internal void SpeachRequest(string text)
        {
            //Vozes em portugues: http://www.microsoft.com/downloads/details.aspx?FamilyID=30e14c5a-a42c-4d4e-9513-c4b0b8d21086&displaylang=en
            if (Synthetizer.Voice.Culture.CompareInfo.LCID != 1046)
            {
                foreach (InstalledVoice voice in Synthetizer.GetInstalledVoices())
                {
                    if (voice.VoiceInfo.Culture.CompareInfo.LCID == 1046)
                    {
                        Synthetizer.SelectVoice(voice.VoiceInfo.Name);
                        break;
                    }
                }
            }
            Synthetizer.SpeakAsync(text);
        }

        internal void SpeachStop()
        {
            Synthetizer.SpeakAsyncCancelAll();
        }
        #endregion

        #region Content processing
        public void ShowContent(SbResultset results)
        {
            this.mainPage.ClearContent();
            foreach(ISbItem item in results.Itens)
                ActivateSbItem(item);
            this.mainPage.ShowContent(results);
        }

        internal void ShowHelp()
        {
            this.ClosePrincipal();
            this.mainPage.ShowHelp();
        }
        #endregion

        #region Search processing
        internal void SearchChanged(AutoComplete component, string term)
        {
            IEnumerable<string> sugestoes = this.textSuggest.GetSuggestionsFor(term);
            component.ItemsSource = sugestoes;
        }

        internal void SearchRequested(string term)
        {
            if (this.sbItemSuggest.GetSuggestionsFor(term).Count() > 0)
            {
                this.controller.DefaultTerm = term;
                IEnumerable<ISbItem> opcao = this.sbItemSuggest.GetSuggestionsFor(term).First();
                this.ShowContent(new SbResultset(opcao, SbResultsetType.Referencia));
            }
            else
            {
                mainPage.ShowHelp(true);
            }
        }

        internal void BuscaRequested(string phrase)
        {
            /*IEnumerable<ISbItem> versiculos = 
                from Versiculo v in controller.DefaultContainer
                where v.Descricao.Contains(phrase)
                select v as ISbItem;*/

            IList<ISbItem> versiculos = new List<ISbItem>();

            IndexSearcher searcher = this.controller.Index.GetIndex();
            QueryParser queryParser = new QueryParser("versiculo", new StandardAnalyzer());
            Hits hits = searcher.Search(queryParser.Parse(phrase));
            for (int i = 0; i < (hits.Length() > 100 ? 100 : hits.Length()); i++)
            {
                ISbItem item = this.controller.DefaultContainer.Ext().GetByID(Convert.ToInt64(hits.Doc(i).Get("id"))) as ISbItem;
                item.Tag = hits.Score(i).ToString();
                this.controller.DefaultContainer.Activate(item, 1);
                versiculos.Add(item);
            }

            this.ShowContent(new SbResultset(versiculos, SbResultsetType.BuscaLivre));
        }
        #endregion

        #region Shortcuts view
        internal void ShowHideShortcuts()
        {
            if ((string)shortcutsPage.showHideButton.Content == ">>")
            {
                DefaultEffects.MoveShortcuts(shortcutsPage, "conteudo", 0);
                shortcutsPage.showHideButton.Content = "<<";
                shortcutsPage.tabs.IsEnabled = true;
                AutomationProperties.SetName(shortcutsPage.showHideButton, "Voltar à tela Principal");
            }
            else
            {
                DefaultEffects.MoveShortcuts(shortcutsPage, "conteudo", -380);
                shortcutsPage.showHideButton.Content = ">>";
                shortcutsPage.tabs.IsEnabled = false;
                AutomationProperties.SetName(shortcutsPage.showHideButton, "Acessar índice de Livros");
            }
        }

        internal void ShowContentFromShortcuts(Livro livro)
        {
            this.ClosePrincipal();
            this.mainPage.busca.PopupEnabled = false;
            this.mainPage.busca.Text = livro.Display;
            this.mainPage.busca.PopupEnabled = true;
            this.ShowContent(new SbResultset(livro.Children, SbResultsetType.Referencia));
            Keyboard.Focus(this.mainPage.ler);
        }
        #endregion

        #region FullScreen
        internal bool FullScreen
        {
            get { return fullScreen; }
            set
            {
                fullScreen = value;
                if (value)
                {
                    lastWindowState = this.mainWindow.WindowState;
                    lastWindowStyle = this.mainWindow.WindowStyle;

                    this.mainWindow.WindowState = WindowState.Normal;
                    this.mainWindow.WindowStyle = WindowStyle.None;
                    this.mainWindow.Topmost = true;
                    this.mainWindow.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.mainWindow.WindowStyle = lastWindowStyle;
                    this.mainWindow.Topmost = false;
                    this.mainWindow.WindowState = lastWindowState;
                }
            }
        }
        #endregion

        #region General functions
        internal void ActivateSbItem(ISbItem item)
        {
            this.controller.DefaultContainer.Activate(item, 5);
        }

        internal void ClosePrincipal()
        {
            if (this.mainWindow.principal.NavigationService.Content != this.mainPage)
            {
                this.mainWindow.principal.Navigate(this.mainPage);
                this.mainPage.busca.Text = this.principalPage.busca.Text;
            }
        }
        #endregion

    }
}
