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
using System.Speech.Synthesis;
using System.Windows.Controls;
using AltzControls;
using System.Windows.Automation;
using System.Windows.Input;

namespace SpokenBible.Presenter
{
    public class MainPresenter
    {
        private MainWindow mainWindow = null;
        private Main mainPage = null;
        private Principal principalPage = null;
        private Shortcuts shortcutsPage = null;
        private AppController controller = null;
        
        private ISuggestComponent<string> textSuggest = null;
        private ISuggestComponent<IEnumerable<ISbItem>> sbItemSuggest = null;

        private SpeechSynthesizer synthetizer = null;

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
            IEnumerable<Livro> livros = from Livro l in controller.DefaultContainer
                                        select l;

            this.textSuggest = new SimpleTextSuggester(livros, ActivateSbItem);
            this.sbItemSuggest = new SimpleSbItemSuggester(livros, ActivateSbItem);

            this.shortcutsPage.VelhoTestamento = from Livro l in livros
                                                 where l.Testamento.Acronimo == "AT"
                                                 select l;
            this.shortcutsPage.NovoTestamento = from Livro l in livros
                                                 where l.Testamento.Acronimo == "NT"
                                                 select l;

        }

        private SpeechSynthesizer Synthetizer
        {
            get
            {
                if (synthetizer == null)
                    synthetizer = new SpeechSynthesizer();
                return synthetizer;
            }
        }

        public void ShowView()
        {
            mainWindow.Show();
        }

        public void ShowContent(IEnumerable<ISbItem> itens)
        {
            foreach(ISbItem item in itens)
                this.ActivateSbItem(item);
            this.mainPage.ClearContent();
            this.mainPage.ShowContent(itens);
        }

        public void ShowContent(ISbItem item)
        {
            IList<ISbItem> resp = new List<ISbItem>();
            resp.Add(item);
            this.ShowContent(resp);
        }

        internal void ShowHelp()
        {
            this.ClosePrincipal();
            this.mainPage.ShowHelp();
        }
        
        internal void SearchChanged(AutoComplete component, string term)
        {
            IEnumerable<string> sugestoes = this.textSuggest.GetSuggestionsFor(term);
            component.ItemsSource = sugestoes;
        }

        internal void ActivateSbItem(ISbItem item)
        {
            this.controller.DefaultContainer.Activate(item, 5);
        }

        internal void SearchRequested(string term)
        {
            if (this.sbItemSuggest.GetSuggestionsFor(term).Count() > 0)
            {
                IEnumerable<ISbItem> opcao = this.sbItemSuggest.GetSuggestionsFor(term).First();
                this.ShowContent(opcao);
            }
            else
            {
                ShowContent(new Livro(0, "Nao", "Não encontrado"));
            }
        }

        internal void SpeachRequest(string text)
        {
            //Vozes em portugues: http://www.microsoft.com/downloads/details.aspx?FamilyID=30e14c5a-a42c-4d4e-9513-c4b0b8d21086&displaylang=en
            IList<InstalledVoice> voices = Synthetizer.GetInstalledVoices();
            //Synthetizer.Rate = -2;
            Synthetizer.SpeakAsync(text);
        }

        internal void SpeachStop()
        {
            Synthetizer.SpeakAsyncCancelAll();
        }

        #region principal
        internal void ClosePrincipal()
        {
            this.mainWindow.principal.Navigate(this.mainPage);
            this.mainPage.busca.Text = this.principalPage.busca.Text;
        }
        #endregion

        #region shortcuts
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
            this.ShowContent(livro);
            Keyboard.Focus(this.mainPage.ler);
        }
        #endregion

    }
}
