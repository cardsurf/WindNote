using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WindNote.Gui.CustomControls
{
    class TextBoxHintTextChangedHandler
    {
        private TextBoxHint TextBox;

        public TextBoxHintTextChangedHandler(TextBoxHint textBox)
        {
            this.TextBox = textBox;
        }

        public void ShowHintTextWhenEmpty(object sender, RoutedEventArgs e)
        {
            this.ShowHintTextWhenEmpty(sender, null);
        }

        public void ShowHintTextWhenEmpty(object sender, TextChangedEventArgs e)
        {
            if (TextBox.Text.Length == 0)
            {
                this.ShowHintText();
            }
            else
            {
                this.HideHintText();
            }
        }

        private void ShowHintText()
        {
            if (TextBox.HintText != null)
            {
                this.SetHintLabelText();
                this.SwapTextBoxBackground();
                this.SetTextBoxWidthToHintText();
            }
        }

        private void SetHintLabelText()
        {
            this.TextBox.HintLabel.Content = this.TextBox.HintText;
        }

        private void SwapTextBoxBackground()
        {
            this.TextBox.PreviousBackground = this.TextBox.Background;
            this.TextBox.Background = this.TextBox.HintBackground;
        }

        private void SetTextBoxWidthToHintText()
        {
            Size textSize = this.MeasureTextBoxTextSize(TextBox.HintText);
            this.TextBox.Width = textSize.Width;
        }

        private Size MeasureTextBoxTextSize(string text)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(this.TextBox.FontFamily, this.TextBox.FontStyle, this.TextBox.FontWeight, this.TextBox.FontStretch),
                this.TextBox.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private void HideHintText()
        {
            this.TextBox.HintLabel.Content = "";
            this.TextBox.Background = this.TextBox.PreviousBackground;
            this.SetTextBoxWidthToAuto();
        }

        private void SetTextBoxWidthToAuto()
        {
            this.TextBox.Width = Double.NaN;
        }

    }
}
