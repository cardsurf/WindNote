using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WindNote.Gui.CustomControls
{
    class TextBoxHint : TextBox
    {
        public String HintText
        {
            get { return (String)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }
        public static readonly DependencyProperty HintTextProperty =
            DependencyProperty.Register("HintText", typeof(String), typeof(TextBoxHint));

        public Label HintLabel;
        public VisualBrush HintBackground;
        public Brush PreviousBackground;

        public TextBoxHint()
        {
            this.InitHintLabel();
            this.InitHintBackground();
            this.AddLoadedHandler();
            this.AddTextChangedHandler();
            this.AddTripleClickHandler();
        }

        private void InitHintLabel()
        {
            this.HintLabel = new Label();
            this.HintLabel.Foreground = Brushes.LightGray;
            this.HintLabel.Padding = new Thickness(0);
            this.HintLabel.Margin = new Thickness(0);
        }

        private void InitHintBackground()
        {
            this.HintBackground = new VisualBrush();
            this.HintBackground.Stretch = Stretch.None;
            this.HintBackground.Visual = HintLabel;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetHintBackgroundAlignment();
        }

        private void SetHintBackgroundAlignment()
        {
            this.HintBackground.AlignmentX = this.GetContentAlignmentX();
            this.HintBackground.AlignmentY = this.GetContentAlignmentY();
        }

        private AlignmentX GetContentAlignmentX()
        {
            switch(this.HorizontalContentAlignment)
            {
                case HorizontalAlignment.Left:
                    return AlignmentX.Left;
                case HorizontalAlignment.Center:
                    return AlignmentX.Center;
                case HorizontalAlignment.Right:
                    return AlignmentX.Right;
                default:
                    return AlignmentX.Center;
            }
        }

        private AlignmentY GetContentAlignmentY()
        {
            switch(this.VerticalContentAlignment)
            {
                case VerticalAlignment.Top:
                    return AlignmentY.Top;
                case VerticalAlignment.Center:
                    return AlignmentY.Center;
                case VerticalAlignment.Bottom:
                    return AlignmentY.Bottom;
                default:
                    return AlignmentY.Center;
            }
        }

        private void AddLoadedHandler()
        {
            TextBoxHintTextChangedHandler handler = new TextBoxHintTextChangedHandler(this);
            this.Loaded += handler.ShowHintTextWhenEmpty;
        }

        private void AddTextChangedHandler()
        {
            TextBoxHintTextChangedHandler handler = new TextBoxHintTextChangedHandler(this);
            this.TextChanged += handler.ShowHintTextWhenEmpty;
            this.TextChanged += this.RemoveTildeKeyChars;
        }

        private void RemoveTildeKeyChars(object sender, TextChangedEventArgs e)
        {
            String text = this.Text;
            text = text.Replace("`", String.Empty);
            text = text.Replace("~", String.Empty);
            this.ReplaceText(text);
        }

        private void ReplaceText(String filteredText)
        {
            int charsRemoved = this.Text.Length - filteredText.Length;
            if(charsRemoved > 0)
            {
                int newIndex = this.CaretIndex - charsRemoved;
                this.Text = filteredText;
                this.CaretIndex = newIndex;
            }
        }

        private void AddTripleClickHandler()
        {
            this.PreviewMouseDown += SelectAllTextOnTripleClick;
        }

        private void SelectAllTextOnTripleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
            {
                this.SelectAll();
            }
        }

        public void SetKeyboardFocus()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(delegate()
            {
                this.Focus();
                this.SelectionStart = this.Text.Length;
            }));
        }

    }
}
