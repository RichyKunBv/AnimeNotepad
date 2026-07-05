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

    private async void MenuPrint_Click(object? sender, RoutedEventArgs e)
    {
        var printWindow = new PrintWindow();
        var result = await printWindow.ShowDialog<bool>(this);
        
        if (result)
        {
            string textToPrint = EditorTextBox.Text ?? string.Empty;
            try
            {
                string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "AnimeNotepad_Print.txt");
                System.IO.File.WriteAllText(tempFile, textToPrint);
                
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "lpr",
                        Arguments = $"\"{tempFile}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                    ShowMessage("Impresión", "Documento enviado a la impresora de macOS.");
                }
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "notepad",
                        Arguments = $"/p \"{tempFile}\"",
                        UseShellExecute = true,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                    });
                    ShowMessage("Impresión", "Documento enviado a la impresora de Windows.");
                }
                else
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "lpr",
                        Arguments = $"\"{tempFile}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                    ShowMessage("Impresión", "Documento enviado a lpr en Linux.");
                }
            }
            catch (System.Exception ex)
            {
                ShowMessage("Error de Impresión", $"No se pudo completar la impresión: {ex.Message}");
            }
        }
    }

    private void MenuExit_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void MenuCut_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Cut();
    private void MenuCopy_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Copy();
    private void MenuPaste_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Paste();
    private void MenuSelectAll_Click(object? sender, RoutedEventArgs e) => EditorTextBox.SelectAll();

    private async void MenuFonts_Click(object? sender, RoutedEventArgs e)
    {
        var fontWindow = new FontColorWindow(
            EditorTextBox.FontFamily,
            EditorTextBox.FontSize,
            EditorTextBox.FontWeight,
            EditorTextBox.FontStyle,
            null, // TextDecorations underline not strictly natively supported directly on TextBox, but we can pass null for now
            EditorTextBox.Foreground
        );

        var result = await fontWindow.ShowDialog<FontColorDialogResult>(this);
        if (result != null)
        {
            EditorTextBox.FontFamily = result.FontFamily ?? EditorTextBox.FontFamily;
            EditorTextBox.FontSize = result.FontSize;
            EditorTextBox.FontWeight = result.FontWeight;
            EditorTextBox.FontStyle = result.FontStyle;
            // Note: TextBox in Avalonia does not currently support TextDecorations (underline) easily on the whole control. 
            // We apply the Foreground.
            if (result.Foreground != null)
            {
                EditorTextBox.Foreground = result.Foreground;
            }
        }
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

    private async void MenuManual_Click(object? sender, RoutedEventArgs e)
    {
        var manualWindow = new ManualWindow();
        await manualWindow.ShowDialog(this);
    }

    private async void MenuAbout_Click(object? sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow();
        await aboutWindow.ShowDialog(this);
    }

    private async void MenuCheckUpdates_Click(object? sender, RoutedEventArgs e)
    {
        var (status, latestVersion) = await AnimeNotepad.Services.UpdateService.CheckForUpdatesAsync(Verzion.Texto.TrimStart('v', 'V'));

        if (status == AnimeNotepad.Services.UpdateStatus.Outdated)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = AnimeNotepad.Services.UpdateService.GetDirectDownloadUrl(),
                    UseShellExecute = true
                });
                ShowMessage("Actualización en curso", $"Se ha iniciado la descarga de la nueva versión ({latestVersion}).");
            }
            catch
            {
                ShowMessage("Actualización disponible", $"Hay una nueva versión disponible ({latestVersion}).\nVisita el repositorio para descargarla.");
            }
        }
        else if (status == AnimeNotepad.Services.UpdateStatus.UpToDate || status == AnimeNotepad.Services.UpdateStatus.Newer)
        {
            ShowMessage("Actualizaciones", "Ya tienes la última versión instalada.");
        }
        else
        {
            ShowMessage("Error", "No se pudo verificar la actualización.");
        }
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