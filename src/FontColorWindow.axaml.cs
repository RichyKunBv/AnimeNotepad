using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace AnimeNotepad
{
    public class FontColorDialogResult
    {
        public FontFamily? FontFamily { get; set; }
        public double FontSize { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStyle FontStyle { get; set; }
        public TextDecorationCollection? TextDecorations { get; set; }
        public ISolidColorBrush? Foreground { get; set; }
    }

    public partial class FontColorWindow : Window
    {
        public FontColorDialogResult? Result { get; private set; }
        private ISolidColorBrush _selectedColor = Brushes.Black;

        public FontColorWindow()
        {
            InitializeComponent();

            var fontFamilyComboBox = this.FindControl<ComboBox>("FontFamilyComboBox");
            if (fontFamilyComboBox != null)
            {
                var fonts = FontManager.Current.SystemFonts.Select(f => f.Name).OrderBy(f => f).ToList();
                fontFamilyComboBox.ItemsSource = fonts;
                fontFamilyComboBox.SelectedItem = "Segoe UI";
            }
        }

        public FontColorWindow(FontFamily currentFont, double currentSize, FontWeight currentWeight, FontStyle currentStyle, TextDecorationCollection? currentDecorations, IBrush? currentBrush) : this()
        {
            var fontFamilyComboBox = this.FindControl<ComboBox>("FontFamilyComboBox");
            if (fontFamilyComboBox != null && currentFont != null)
                fontFamilyComboBox.SelectedItem = currentFont.Name;

            var fontSizeUpDown = this.FindControl<NumericUpDown>("FontSizeUpDown");
            if (fontSizeUpDown != null)
                fontSizeUpDown.Value = (decimal)currentSize;

            var chkBold = this.FindControl<CheckBox>("ChkBold");
            if (chkBold != null)
                chkBold.IsChecked = currentWeight == FontWeight.Bold;

            var chkItalic = this.FindControl<CheckBox>("ChkItalic");
            if (chkItalic != null)
                chkItalic.IsChecked = currentStyle == FontStyle.Italic;

            var chkUnderline = this.FindControl<CheckBox>("ChkUnderline");
            if (chkUnderline != null)
                chkUnderline.IsChecked = currentDecorations != null && currentDecorations.Count > 0;

            if (currentBrush is ISolidColorBrush scb)
            {
                _selectedColor = scb;
            }
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Background is ISolidColorBrush brush)
            {
                _selectedColor = brush;
            }
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            var fontFamilyStr = this.FindControl<ComboBox>("FontFamilyComboBox")?.SelectedItem as string;
            var fontSizeUpDown = this.FindControl<NumericUpDown>("FontSizeUpDown");
            var chkBold = this.FindControl<CheckBox>("ChkBold");
            var chkItalic = this.FindControl<CheckBox>("ChkItalic");
            var chkUnderline = this.FindControl<CheckBox>("ChkUnderline");

            Result = new FontColorDialogResult
            {
                FontFamily = string.IsNullOrEmpty(fontFamilyStr) ? FontFamily.Default : new FontFamily(fontFamilyStr),
                FontSize = fontSizeUpDown != null ? (double)(fontSizeUpDown.Value ?? 14) : 14,
                FontWeight = chkBold?.IsChecked == true ? FontWeight.Bold : FontWeight.Normal,
                FontStyle = chkItalic?.IsChecked == true ? FontStyle.Italic : FontStyle.Normal,
                TextDecorations = chkUnderline?.IsChecked == true ? Avalonia.Media.TextDecorations.Underline : null,
                Foreground = _selectedColor
            };

            Close(Result);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Result = null;
            Close();
        }
    }
}
