using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using agsXMPP;
using agsXMPP.protocol.extensions.nickname;
using agsXMPP.protocol.client;
using SBRobots.Properties;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using sbcore.Persistence;
using sbcore.Components;
using sbcore.Components.Interface;
using sbcore.Model.Interface;
using sbcore.Model;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using System.Text;

namespace SBRobots
{
    public class Controller
    {
        #region Attributes and Properties
        private ISuggestComponent<ISbItem> sbItemSuggest = null;
        private Index DefaultIndex { get; set; }
        private HttpApplicationState Application { get; set; }
        private IObjectContainer DefaultContainer { get; set; }
        
        public XmppClientConnection Xmpp
        {
            get
            {
                XmppClientConnection xmpp = (XmppClientConnection)Application["xmpp"];
                if (xmpp == null)
                {
                    xmpp = new XmppClientConnection();
                    Application["xmpp"] = xmpp;
                }
                return xmpp;
            }
        }
        #endregion

        #region Constructor
        public Controller(HttpApplicationState Application)
        {
            this.Application = Application;
            this.DefaultContainer = Container.GetContainer(HttpContext.Current.Server.MapPath(Resources.DatabaseFile));
            this.DefaultIndex = new Index(HttpContext.Current.Server.MapPath(Resources.IndexFile));

            IEnumerable<Livro> livros = from Livro l in DefaultContainer
                                        select l;
            this.sbItemSuggest = new SimpleSbItemSuggester(livros, ActivateSbItem);
        }
        #endregion

        #region Server Controll
        public void StartServer()
        {
            Xmpp.OnLogin += new ObjectHandler(xmpp_OnLogin);
            Xmpp.OnMessage += new agsXMPP.protocol.client.MessageHandler(xmpp_OnMessage);
            Xmpp.AutoPresence = false;

            Xmpp.Port = 5222;
            Xmpp.UseSSL = false;
            Xmpp.Server = Resources.BotDomain;
            Xmpp.ConnectServer = Resources.BotServer;

            Xmpp.Open(Resources.BotUserName, Resources.BotPassword);
        }

        public void StopServer()
        {
            Xmpp.Close();
        }
        #endregion

        #region Search processing
        internal SbResultset SearchRequested(string term)
        {
            if (this.sbItemSuggest.GetSuggestionsFor(term).Count() > 0)
            {
                ISbItem opcao = this.sbItemSuggest.GetSuggestionsFor(term).First();
                return new SbResultset(opcao, SbResultsetType.Referencia);
            }

            try
            {
                int total = 0;
                IList<ISbItem> versiculos = BuscaRequested(term, out total);
                if (versiculos.Count > 0)
                {
                    SbResultset resultset = new SbResultset(versiculos, SbResultsetType.BuscaLivre);
                    resultset.TotalSearchResults = total;
                    return resultset;
                }
            }
            catch { }

            return null;
            //this.ShowHelp();
        }

        internal IList<ISbItem> BuscaRequested(string phrase, out int total)
        {
            IList<ISbItem> versiculos = new List<ISbItem>();

            IndexSearcher searcher = this.DefaultIndex.GetIndex();
            QueryParser queryParser = new QueryParser("versiculo", new StandardAnalyzer());
            Hits hits = searcher.Search(queryParser.Parse(phrase));
            total = hits.Length();
            for (int i = 0; i < (hits.Length() > 100 ? 100 : hits.Length()); i++)
            {
                ISbItem item = this.DefaultContainer.Ext().GetByID(Convert.ToInt64(hits.Doc(i).Get("id"))) as ISbItem;
                item.Tag = hits.Score(i).ToString();
                this.DefaultContainer.Activate(item, 1);
                versiculos.Add(item);
            }

            return versiculos;
        }
        #endregion

        #region Events
        private void xmpp_OnMessage(object sender, agsXMPP.protocol.client.Message msg)
        {
            StringBuilder sb = new StringBuilder();
            SbResultset resultset = SearchRequested(msg.Body);
            if (resultset == null)
            {
                Xmpp.Send(new Message(msg.From, "Não entendi, por favor, reformule sua consulta.\nEx: João 3:16 ou Salmo 23:1-4"));
                return;
            }

            foreach (ISbItem item in resultset.Itens.Take(10))
            {
                if ((item is Livro) || (item is Capitulo))
                {
                    ActivateSbItem(item);
                    sb.Append(string.Format(GetTitle(item), item.Children.Count()));
                    sb.Append("\nSeja mais específico na sua consulta.\n");
                }
                else if (item is Versiculo)
                {
                    sb.Append(GetTitle(item));
                    sb.Append(item.Display);
                    sb.Append("\n");
                }
                else if (item is SbItemPair)
                {
                    foreach (ISbItem child in item.Children)
                    {
                        sb.Append(GetTitle(child));
                        sb.Append(" ");
                        sb.Append(child.Display);
                        sb.Append("\n");
                    }
                }
            }
            Xmpp.Send(new Message(msg.From, sb.ToString()));
        }

        private void xmpp_OnLogin(object sender)
        {
            Presence p = new Presence();
            p.Nickname = new Nickname("A Bíblia Falada (a)");
            p.Status = "João 3.16: \"Porque Deus amou ao mundo de tal maneira que deu o seu Filho unigênito, para que todo o que nele crê não pereça, mas tenha a vida eterna.\"";

            Xmpp.Send(p);
            Xmpp.OnPresence += new PresenceHandler(xmpp_OnPresence);
        }

        private void xmpp_OnPresence(object sender, Presence pres)
        {
            if (pres.Type == PresenceType.subscribe)
            {
                Xmpp.PresenceManager.ApproveSubscriptionRequest(pres.From);
            }
            else if (pres.Type == PresenceType.available)
            {
                //Xmpp.Send(new Message(pres.From, "Versículo de hoje: \"Porque Deus amou ao mundo de tal maneira que deu o seu Filho unigênito, para que todo o que nele crê não pereça, mas tenha a vida eterna.\" João 3:16"));
            }
        }
        #endregion

        #region General functions
        internal void ActivateSbItem(ISbItem item)
        {
            this.DefaultContainer.Activate(item, 5);
        }

        private string GetTitle(ISbItem item)
        {
            if (item == null)
                return string.Empty;

            if (item is Livro)
                return item.Display + " possui {0:d} capítulos.";
            if (item is Capitulo)
                return item.Parent.Display + " " + item.Display + " possui {0:d} versículos.";
            else
                return item.Parent.Parent.Display + " " + item.Parent.Display + ":";
        }
        #endregion
    }
}
