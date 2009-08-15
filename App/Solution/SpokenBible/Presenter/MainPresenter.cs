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

namespace SpokenBible.Presenter
{
    public class MainPresenter
    {
        private MainWindow mainWindow = null;
        private Principal principalPage = null;
        private Shortcuts shortcutsPage = null;
        private AppController controller = null;
        
        private ISuggestComponent<string> textSuggest = null;
        private ISuggestComponent<ISbItem> sbItemSuggest = null;

        private SpeechSynthesizer synthetizer = null;

        public MainPresenter(AppController controller)
        {
            //inicialização dos controles
            this.controller = controller;
            this.mainWindow = new MainWindow(this);
            this.principalPage = new Principal(this);
            this.shortcutsPage = new Shortcuts(this);

            //carregamento de paginas e conteudos visuais adicionais
            this.mainWindow.SetNavigationPages(this.principalPage, this.shortcutsPage);

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

        public void ShowContent(ISbItem item)
        {
            this.controller.DefaultContainer.Activate(item, 5);
            mainWindow.ClearContent();
            mainWindow.ShowContent(item);
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
                ISbItem opcao = this.sbItemSuggest.GetSuggestionsFor(term).First();
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

        #region principal
        internal void ClosePrincipal()
        {
            DefaultEffects.HidePrincipal(mainWindow, "principal");
        }
        #endregion

        #region shortcuts
        internal void ShowHideShortcuts()
        {
            if ((string)shortcutsPage.showHideButton.Content == ">>")
            {
                DefaultEffects.MoveShortcuts(shortcutsPage, "conteudo", 0);
                shortcutsPage.showHideButton.Content = "<<";
            }
            else
            {
                DefaultEffects.MoveShortcuts(shortcutsPage, "conteudo", -380);
                shortcutsPage.showHideButton.Content = ">>";
            }
        }
        #endregion
    }
}
