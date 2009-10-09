using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using agsXMPP;
using agsXMPP.protocol.extensions.nickname;
using agsXMPP.protocol.client;
using SBRobots.Properties;
using sbcore.Persistence;
using sbcore.Components;
using sbcore.Components.Interface;
using sbcore.Model.Interface;
using sbcore.Model;
using System.Text;
using SBRobots.Bots;

namespace SBRobots
{
    public class Controller
    {
        #region Attributes and Properties
        private HttpApplicationState Application { get; set; }
        private Searcher searcher = null;
        private string customMessage = "\"Porque Deus amou ao mundo de tal maneira que deu o seu Filho unigênito, para que todo o que nele crê não pereça, mas tenha a vida eterna.\" João 3:16";
        
        public IBot[] Bots
        {
            get
            {
                return (IBot[])Application["bots"];
            }
            set
            {
                Application["bots"] = value;
            }
        }
        #endregion

        #region Constructor
        public Controller(HttpApplicationState Application)
        {
            this.Application = Application;
            searcher = new Searcher(HttpContext.Current.Server.MapPath(Resources.DatabaseFile), HttpContext.Current.Server.MapPath(Resources.IndexFile));
        }
        #endregion

        #region Server Controll
        public void StartAll()
        {
            if (Bots == null)
                Bots = new IBot[2];

            if (Bots[0] == null)
            {
                Bots[0] = new GTalkBot();

                Bots[0].OnMessage += new BotMessageHandler(this.OnMessage);
                Bots[0].CustomMessage = this.customMessage;

                Bots[0].BotDomain = Resources.BotDomain;
                Bots[0].BotServer = Resources.BotServer;
                Bots[0].BotUserName = Resources.BotUserName;
                Bots[0].BotPassword = Resources.BotPassword;
            }
            Bots[0].Start();

            if (Bots[1] == null)
            {
                Bots[1] = new MsnBot();

                Bots[1].OnMessage += new BotMessageHandler(this.OnMessage);
                Bots[1].CustomMessage = this.customMessage;

                /*Bots[1].BotDomain = Resources.BotDomain;
                Bots[1].BotServer = Resources.BotServer;*/
                Bots[1].BotUserName = "@hotmail.com";
                Bots[1].BotPassword = "";
            }
            Bots[1].Start();
        }

        public void StopAll()
        {
            foreach (IBot bot in this.Bots)
            {
                bot.Stop();
            }
        }
        #endregion

        #region Events
        private string OnMessage(object sender, string term)
        {
            StringBuilder sb = new StringBuilder();
            SbResultset resultset = searcher.Search(term);
            if (resultset == null)
            {
                return "Não entendi, por favor, reformule sua consulta.\nEx: João 3:16 ou Salmo 23:1-4";
            }

            foreach (ISbItem item in resultset.Itens.Take(10))
            {
                if ((item is Livro) || (item is Capitulo))
                {
                    searcher.ActivateSbItem(item);
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
            return sb.ToString();
        }

        #endregion

        #region General functions
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
