using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindNote.Gui.AttachedBehaviors
{
    class TextBoxIfEmptyKeyDownExecute
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand), typeof(TextBoxIfEmptyKeyDownExecute), new UIPropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
             typeof(object), typeof(TextBoxIfEmptyKeyDownExecute), new UIPropertyMetadata(null));

        public static DependencyProperty KeyProperty =
            DependencyProperty.RegisterAttached("Key",
             typeof(Key), typeof(TextBoxIfEmptyKeyDownExecute), new UIPropertyMetadata(null));


        public static void SetCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        public static void SetCommandParameter(DependencyObject element, object value)
        {
            element.SetValue(CommandParameterProperty, value);
        }

        public static object GetCommandParameter(DependencyObject element)
        {
            return element.GetValue(CommandParameterProperty);
        }

        public static void SetKey(DependencyObject element, Key value)
        {
            element.SetValue(KeyProperty, value);
        }

        public static Key GetKey(DependencyObject element)
        {
            return (Key) element.GetValue(KeyProperty);
        }





        private static void CommandChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = source as UIElement;
            if (CommandPropertyAdded(element, e))
            {
                element.PreviewKeyDown += ExecuteCommand;
            }
            else if (CommandPropertyRemoved(element, e))
            {
                element.PreviewKeyDown -= ExecuteCommand;
            }
        }

        private static bool CommandPropertyAdded(UIElement element, DependencyPropertyChangedEventArgs e)
        {
            return (element != null) && (e.NewValue != null) && (e.OldValue == null);
        }

        private static bool CommandPropertyRemoved(UIElement element, DependencyPropertyChangedEventArgs e)
        {
            return (element != null) && (e.NewValue == null) && (e.OldValue != null);
        }

        private static void ExecuteCommand(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Key key = (Key) textBox.GetValue(KeyProperty);
            if (e.Key == key && textBox.Text.Length == 0)
            {
                e.Handled = true;
                ICommand command = (ICommand) textBox.GetValue(CommandProperty);
                object commandParameter = textBox.GetValue(CommandParameterProperty);
                command.Execute(commandParameter);
            }
        }

    }
}
