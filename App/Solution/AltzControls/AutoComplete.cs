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

/*
 * Seguindo o tutorial:
 * http://xamlcoder.com/cs/blogs/joe/archive/2007/12/13/building-custom-template-able-wpf-controls.aspx
 */
namespace AltzControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AltzControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AltzControls;assembly=AltzControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:AutoComplete/>
    ///
    /// </summary>
    public class AutoComplete : Control
    {
        static AutoComplete()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoComplete), new FrameworkPropertyMetadata(typeof(AutoComplete)));
        }

        #region Text property
        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(String), typeof(AutoComplete),
                new UIPropertyMetadata(null,
                    new PropertyChangedCallback(OnTextChanged),
                    new CoerceValueCallback(OnCoerceText)));

        private static object OnCoerceText(DependencyObject o, Object value)
        {
            if (o != null)
            {
                AutoComplete autoComplete = o as AutoComplete;
                return autoComplete.OnCoerceText((string)value);
            }
            else
                return value;
        }

        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                AutoComplete autoComplete = o as AutoComplete;
                autoComplete.OnTextChanged((string)e.OldValue, (string)e.NewValue);
            }
        }

        protected virtual String OnCoerceText(String value)
        {
            return value;
        }

        protected virtual void OnTextChanged(String oldValue, String newValue)
        {
            this.RaiseEvent(new RoutedEventArgs(AutoComplete.TextChangedEvent, this));
        }

        public String Text
        {
            // IMPORTANT: To maintain parity between setting a property in XAML
            // and procedural code, do not touch the getter and setter inside
            // this dependency property!
            get
            {
                return (String)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        #endregion

        #region TextChanged event
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent("TextChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(AutoComplete));

        public event RoutedEventHandler TextChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }
        #endregion
    }
}
