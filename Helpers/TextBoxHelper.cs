using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Helpers
{
    public static class TextBoxHelper
    {
        #region PlaceholderText

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.RegisterAttached(
                "PlaceholderText",
                typeof(string),
                typeof(TextBoxHelper),
                new PropertyMetadata(string.Empty, OnPlaceholderTextChanged));

        public static string GetPlaceholderText(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderTextProperty);
        }

        public static void SetPlaceholderText(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderTextProperty, value);
        }

        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus -= TextBox_GotFocus;
                textBox.LostFocus -= TextBox_LostFocus;

                textBox.GotFocus += TextBox_GotFocus;
                textBox.LostFocus += TextBox_LostFocus;

                SetPlaceholder(textBox);
            }
            else if (d is PasswordBox passwordBox)
            {
                passwordBox.GotFocus -= PasswordBox_GotFocus;
                passwordBox.LostFocus -= PasswordBox_LostFocus;

                passwordBox.GotFocus += PasswordBox_GotFocus;
                passwordBox.LostFocus += PasswordBox_LostFocus;

                SetPlaceholder(passwordBox);
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && IsPlaceholderActive(textBox))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SetPlaceholder(textBox);
            }
        }

        private static void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && IsPlaceholderActive(passwordBox))
            {
                passwordBox.Password = string.Empty;
                passwordBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private static void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetPlaceholder(passwordBox);
            }
        }

        private static void SetPlaceholder(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = GetPlaceholderText(textBox);
                textBox.Foreground = SystemColors.GrayTextBrush;
            }
        }

        private static void SetPlaceholder(PasswordBox passwordBox)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBox.Tag = GetPlaceholderText(passwordBox);
                passwordBox.Foreground = SystemColors.GrayTextBrush;
            }
        }

        private static bool IsPlaceholderActive(TextBox textBox)
        {
            return textBox.Text == GetPlaceholderText(textBox);
        }

        private static bool IsPlaceholderActive(PasswordBox passwordBox)
        {
            return passwordBox.Tag as string == GetPlaceholderText(passwordBox);
        }

        #endregion
    }
}
