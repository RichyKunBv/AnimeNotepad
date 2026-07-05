using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Media;

namespace AnimeNotepad;

public partial class MainWindow : Window
{
    private double _zoomLevel = 14;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void MenuNew_Click(object? sender, RoutedEventArgs e)
    {
        EditorTextBox.Text = string.Empty;
    }

    private async void MenuOpen_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Abrir archivo",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("Text Documents") { Patterns = new[] { "*.txt" } }, new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } } }
        });

        if (files.Count >= 1)
        {
            using var stream = await files[0].OpenReadAsync();
            using var reader = new StreamReader(stream);
            EditorTextBox.Text = await reader.ReadToEndAsync();
        }
    }

    private async void MenuSave_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Guardar archivo",
            SuggestedFileName = "Sin titulo 1",
            DefaultExtension = "txt",
            FileTypeChoices = new[] { new FilePickerFileType("Text Documents") { Patterns = new[] { "*.txt" } } }
        });

        if (file != null)
        {
            using var stream = await file.OpenWriteAsync();
            using var writer = new StreamWriter(stream);
            await writer.WriteAsync(EditorTextBox.Text ?? string.Empty);
        }
    }

    private void MenuPrint_Click(object? sender, RoutedEventArgs e)
    {
        // Avalonia no tiene soporte nativo fácil para imprimir de caja como WinForms (PrintDialog) en v11.
        // Simulamos un cuadro de mensaje de error o aviso por ahora, dado que es multiplataforma.
        ShowMessage("Impresión", "La funcionalidad de impresión nativa multiplataforma no está disponible directamente en Avalonia. Se puede integrar con librerías externas o delegar al OS.");
    }

    private void MenuExit_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void MenuCut_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Cut();
    private void MenuCopy_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Copy();
    private void MenuPaste_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Paste();
    private void MenuSelectAll_Click(object? sender, RoutedEventArgs e) => EditorTextBox.SelectAll();

    private void MenuFonts_Click(object? sender, RoutedEventArgs e)
    {
        // En Avalonia no hay FontDialog nativo. 
        // Implementación básica (podría extenderse a usar una ventana FontColorDialogWindow personalizada).
        ShowMessage("Fuentes", "El diálogo de fuentes requiere una implementación personalizada en Avalonia UI. Puedes cambiar la fuente editando el código XAML por ahora.");
    }

    private void MenuColors_Click(object? sender, RoutedEventArgs e)
    {
        // En Avalonia no hay ColorDialog nativo.
        ShowMessage("Colores", "El diálogo de colores requiere una implementación personalizada. El texto actual es el predeterminado del tema.");
    }

    private void MenuZoomIn_Click(object? sender, RoutedEventArgs e)
    {
        if (_zoomLevel < 72)
        {
            _zoomLevel += 2;
            EditorTextBox.FontSize = _zoomLevel;
        }
    }

    private void MenuZoomOut_Click(object? sender, RoutedEventArgs e)
    {
        if (_zoomLevel > 6)
        {
            _zoomLevel -= 2;
            EditorTextBox.FontSize = _zoomLevel;
        }
    }

    private void MenuManual_Click(object? sender, RoutedEventArgs e)
    {
        // Open web browser or local manual
        ShowMessage("Manual", "Abre manual.html");
    }

    private async void MenuAbout_Click(object? sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow();
        await aboutWindow.ShowDialog(this);
    }

    private async void ShowMessage(string title, string message)
    {
        // Basic MessageBox implementation since Avalonia doesn't have one out of the box.
        var msgBox = new Window()
        {
            Title = title,
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Thickness(20),
                Children = 
                {
                    new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,0,0,20) },
                    new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center }
                }
            }
        };

        var btn = (Button)((StackPanel)msgBox.Content).Children[1];
        btn.Click += (_, _) => msgBox.Close();

        await msgBox.ShowDialog(this);
    }
}