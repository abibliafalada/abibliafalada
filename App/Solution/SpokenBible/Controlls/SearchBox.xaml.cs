using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpokenBible.Controlls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
        }

        #region routed events

        public static readonly RoutedEvent FecharButtonClickEvent = EventManager.RegisterRoutedEvent(
            "FecharButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBox));

        public static readonly RoutedEvent BuscarButtonClickEvent = EventManager.RegisterRoutedEvent(
            "BuscarButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchBox));

        // Expose this event for this control's container
        public event RoutedEventHandler FecharButtonClick
        {
            add { AddHandler(FecharButtonClickEvent, value); }
            remove { RemoveHandler(FecharButtonClickEvent, value); }
        }

        public event RoutedEventHandler BuscarButtonClick
        {
            add { AddHandler(BuscarButtonClickEvent, value); }
            remove { RemoveHandler(BuscarButtonClickEvent, value); }
        }

        #endregion

        private void FecharButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FecharButtonClickEvent));
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(BuscarButtonClickEvent));
        }

    }
}
