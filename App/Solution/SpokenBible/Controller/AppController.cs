using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sbcore.Persistence;
using Db4objects.Db4o;
using SpokenBible.Presenter;
using sbcore.Model;
using SpokenBible.Properties;
using SpokenBible.Components;

namespace SpokenBible.Controller
{
    public class AppController
    {
        public IObjectContainer DefaultContainer { get; set; }
        public SBSettings Settings = null;

        public AppController()
        {
            this.Settings = new SBSettings();
        }

        public void Start()
        {
            DefaultContainer = Container.GetContainer(Resources.databaseFile);
            MainPresenter presenter = new MainPresenter(this);
            presenter.ShowView();
        }

        internal void End()
        {
            Container.CloseContainer();
            this.Settings.Save();
        }
    }
}