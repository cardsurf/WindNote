using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WindNote.Gui.AttachedBehaviors
{
    class PreviewMouseDownExecute
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand), typeof(PreviewMouseDownExecute), new UIPropertyMetadata(CommandChanged));


        public static DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter",
             typeof(object), typeof(PreviewMouseDownExecute), new UIPropertyMetadata(null));


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







        private static void CommandChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = source as UIElement;
            if (CommandPropertyAdded(element, e))
            {
                element.PreviewMouseDown += ExecuteCommand;
            }
            else if (CommandPropertyRemoved(element, e))
            {
                element.PreviewMouseDown -= ExecuteCommand;
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

        private static void ExecuteCommand(object sender, RoutedEventArgs e)
        {
            UIElement element = sender as UIElement;
            ICommand command = (ICommand)element.GetValue(CommandProperty);
            object commandParameter = element.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }

    }
}
