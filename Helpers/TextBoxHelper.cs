using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Helpers
{
    public static class TextBoxHelper
    {
        // Dependency Property for Placeholder Text
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.RegisterAttached(
                "PlaceholderText",
                typeof(string),
                typeof(TextBoxHelper),
                new PropertyMetadata(string.Empty, OnPlaceholderTextChanged));

        public static string GetPlaceholderText(FrameworkElement element) => (string)element.GetValue(PlaceholderTextProperty);
        public static void SetPlaceholderText(FrameworkElement element, string value) => element.SetValue(PlaceholderTextProperty, value);

        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus -= RemovePlaceholder;
                textBox.LostFocus -= AddPlaceholder;

                textBox.LostFocus += AddPlaceholder;
                textBox.GotFocus += RemovePlaceholder;

                // Initially set the placeholder
                AddPlaceholder(textBox, null);
            }
            else if (d is PasswordBox passwordBox)
            {
                passwordBox.GotFocus -= RemovePasswordPlaceholder;
                passwordBox.LostFocus -= AddPasswordPlaceholder;

                passwordBox.LostFocus += AddPasswordPlaceholder;
                passwordBox.GotFocus += RemovePasswordPlaceholder;

                // Initially set the placeholder
                AddPasswordPlaceholder(passwordBox, null);
            }
        }

        // TextBox Placeholder Logic
        private static void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = GetPlaceholderText(textBox);
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private static void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == GetPlaceholderText(textBox))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        // PasswordBox Placeholder Logic
        private static void AddPasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && string.IsNullOrEmpty(passwordBox.Password))
            {
                var placeholderText = GetPlaceholderText(passwordBox);
                passwordBox.Tag = placeholderText;
                passwordBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private static void RemovePasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox && (string)passwordBox.Tag == GetPlaceholderText(passwordBox))
            {
                passwordBox.Tag = string.Empty;
                passwordBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }
    }
}
