using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace AnimeNotepad.Views.FontColor
{
    public class FontColorDialogResult
    {
        public FontFamily? FontFamily { get; set; }
        public double FontSize { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStyle FontStyle { get; set; }
        public IBrush? Foreground { get; set; }
        public bool ColorChanged { get; set; }
    }

    public partial class FontColorWindow : Window
    {
        public FontColorDialogResult? Result { get; private set; }
        private IBrush? _selectedColor = null;
        private bool _hasExplicitColor = false;

        public FontColorWindow()
        {
            InitializeComponent();
            SetupFonts(null);
            SetupEventHandlers();
            UpdatePreview();
        }

        public FontColorWindow(FontFamily? currentFont, double currentSize, FontWeight currentWeight, FontStyle currentStyle, TextDecorationCollection? currentDecorations, IBrush? currentBrush)
        {
            InitializeComponent();
            SetupFonts(currentFont);

            var fontSizeUpDown = this.FindControl<NumericUpDown>("FontSizeUpDown");
            if (fontSizeUpDown != null)
                fontSizeUpDown.Value = (decimal)currentSize;

            var chkBold = this.FindControl<CheckBox>("ChkBold");
            if (chkBold != null)
                chkBold.IsChecked = currentWeight == FontWeight.Bold;

            var chkItalic = this.FindControl<CheckBox>("ChkItalic");
            if (chkItalic != null)
                chkItalic.IsChecked = currentStyle == FontStyle.Italic;

            if (currentBrush is ISolidColorBrush scb)
            {
                _selectedColor = scb;
                _hasExplicitColor = true;
            }

            SetupEventHandlers();
            UpdatePreview();
        }

        public void SelectTab(int tabIndex)
        {
            var tabControl = this.FindControl<TabControl>("SettingsTabControl");
            if (tabControl != null && tabIndex >= 0 && tabIndex < tabControl.ItemCount)
            {
                tabControl.SelectedIndex = tabIndex;
            }
        }

        private void SetupFonts(FontFamily? currentFont)
        {
            var fontFamilyComboBox = this.FindControl<ComboBox>("FontFamilyComboBox");
            if (fontFamilyComboBox == null) return;

            var fonts = FontManager.Current.SystemFonts.Select(f => f.Name).OrderBy(f => f).ToList();
            fontFamilyComboBox.ItemsSource = fonts;

            string? targetFontName = currentFont?.Name;
            if (!string.IsNullOrEmpty(targetFontName))
            {
                var match = fonts.FirstOrDefault(f => string.Equals(f, targetFontName, StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    fontFamilyComboBox.SelectedItem = match;
                    return;
                }
            }

            if (fonts.Contains("Inter"))
                fontFamilyComboBox.SelectedItem = "Inter";
            else if (fonts.Contains("Segoe UI"))
                fontFamilyComboBox.SelectedItem = "Segoe UI";
            else if (fonts.Contains("Arial"))
                fontFamilyComboBox.SelectedItem = "Arial";
            else if (fonts.Count > 0)
                fontFamilyComboBox.SelectedIndex = 0;
        }

        private void SetupEventHandlers()
        {
            var fontFamilyComboBox = this.FindControl<ComboBox>("FontFamilyComboBox");
            if (fontFamilyComboBox != null)
                fontFamilyComboBox.SelectionChanged += (_, _) => UpdatePreview();

            var fontSizeUpDown = this.FindControl<NumericUpDown>("FontSizeUpDown");
            if (fontSizeUpDown != null)
                fontSizeUpDown.ValueChanged += (_, _) => UpdatePreview();

            var chkBold = this.FindControl<CheckBox>("ChkBold");
            if (chkBold != null)
                chkBold.IsCheckedChanged += (_, _) => UpdatePreview();

            var chkItalic = this.FindControl<CheckBox>("ChkItalic");
            if (chkItalic != null)
                chkItalic.IsCheckedChanged += (_, _) => UpdatePreview();
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Background != null)
            {
                _selectedColor = btn.Background;
                _hasExplicitColor = true;
                UpdatePreview();
            }
        }

        private void ResetColor_Click(object sender, RoutedEventArgs e)
        {
            _selectedColor = null;
            _hasExplicitColor = false;
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            var preview = this.FindControl<TextBlock>("PreviewTextBlock");
            if (preview == null) return;

            var fontStr = this.FindControl<ComboBox>("FontFamilyComboBox")?.SelectedItem as string;
            if (!string.IsNullOrEmpty(fontStr))
            {
                preview.FontFamily = new FontFamily(fontStr);
            }

            var sizeUpDown = this.FindControl<NumericUpDown>("FontSizeUpDown");
            if (sizeUpDown?.Value != null)
            {
                preview.FontSize = Math.Clamp((double)sizeUpDown.Value.Value, 10, 32);
            }

            var chkBold = this.FindControl<CheckBox>("ChkBold");
            preview.FontWeight = chkBold?.IsChecked == true ? FontWeight.Bold : FontWeight.Normal;

            var chkItalic = this.FindControl<CheckBox>("ChkItalic");
            preview.FontStyle = chkItalic?.IsChecked == true ? FontStyle.Italic : FontStyle.Normal;

            var colorBorder = this.FindControl<Border>("SelectedColorPreview");
            if (_hasExplicitColor && _selectedColor != null)
            {
                preview.Foreground = _selectedColor;
                if (colorBorder != null) colorBorder.Background = _selectedColor;
            }
            else
            {
                preview.ClearValue(TextBlock.ForegroundProperty);
                if (colorBorder != null) colorBorder.Background = Brushes.Transparent;
            }
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            var fontFamilyStr = this.FindControl<ComboBox>("FontFamilyComboBox")?.SelectedItem as string;
            var fontSizeUpDown = this.FindControl<NumericUpDown>("FontSizeUpDown");
            var chkBold = this.FindControl<CheckBox>("ChkBold");
            var chkItalic = this.FindControl<CheckBox>("ChkItalic");

            Result = new FontColorDialogResult
            {
                FontFamily = string.IsNullOrEmpty(fontFamilyStr) ? FontFamily.Default : new FontFamily(fontFamilyStr),
                FontSize = fontSizeUpDown != null ? (double)(fontSizeUpDown.Value ?? 14) : 14,
                FontWeight = chkBold?.IsChecked == true ? FontWeight.Bold : FontWeight.Normal,
                FontStyle = chkItalic?.IsChecked == true ? FontStyle.Italic : FontStyle.Normal,
                Foreground = _selectedColor,
                ColorChanged = _hasExplicitColor
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
