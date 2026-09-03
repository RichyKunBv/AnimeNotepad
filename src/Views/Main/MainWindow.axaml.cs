using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Media;
using Avalonia.Layout;

using AnimeNotepad.Views.About;
using AnimeNotepad.Views.FontColor;
using AnimeNotepad.Views.Manual;
using AnimeNotepad.Views.Print;

namespace AnimeNotepad.Views.Main;

public partial class MainWindow : Window
{
    private string? _currentFilePath = null;
    private bool _isModified = false;
    private bool _isInitializing = true;
    private double _zoomLevel = 14;

    public MainWindow()
    {
        InitializeComponent();

        EditorTextBox.PropertyChanged += (s, e) =>
        {
            if (e.Property == TextBox.CaretIndexProperty)
            {
                UpdateCaretPosition();
            }
        };

        UpdateTitleAndStatus();
        UpdateCaretPosition();
        _isInitializing = false;
    }

    private void EditorTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isInitializing) return;

        if (!_isModified)
        {
            _isModified = true;
        }
        UpdateTitleAndStatus();
    }

    private void UpdateTitleAndStatus()
    {
        string fileName = string.IsNullOrEmpty(_currentFilePath) ? "Sin título" : Path.GetFileName(_currentFilePath);
        string modMarker = _isModified ? "*" : "";
        this.Title = $"{modMarker}{fileName} - AnimeNotepad";

        var statusFileName = this.FindControl<TextBlock>("StatusFileName");
        if (statusFileName != null) statusFileName.Text = fileName;

        var statusDocState = this.FindControl<TextBlock>("StatusDocState");
        if (statusDocState != null) statusDocState.Text = _isModified ? "● Modificado" : "✓ Guardado";

        var statusLength = this.FindControl<TextBlock>("StatusLength");
        if (statusLength != null)
        {
            string text = EditorTextBox.Text ?? string.Empty;
            int lines = text.Length == 0 ? 1 : text.Split('\n').Length;
            statusLength.Text = $"Líneas: {lines} | Caracteres: {text.Length}";
        }
    }

    private void UpdateCaretPosition()
    {
        var statusCursor = this.FindControl<TextBlock>("StatusCursorPosition");
        if (statusCursor == null) return;

        int caret = EditorTextBox.CaretIndex;
        string text = EditorTextBox.Text ?? string.Empty;
        caret = Math.Clamp(caret, 0, text.Length);

        int line = 1;
        int lastNewline = -1;
        for (int i = 0; i < caret; i++)
        {
            if (text[i] == '\n')
            {
                line++;
                lastNewline = i;
            }
        }
        int col = (caret - lastNewline);
        statusCursor.Text = $"Lín {line}, Col {col}";
    }

    private enum ConfirmChoice
    {
        Save,
        Discard,
        Cancel
    }

    private async Task<ConfirmChoice> PromptUnsavedChangesAsync()
    {
        if (!_isModified) return ConfirmChoice.Discard;

        string fileName = string.IsNullOrEmpty(_currentFilePath) ? "Sin título" : Path.GetFileName(_currentFilePath);
        return await ShowConfirmDialogAsync(
            "Guardar cambios",
            $"¿Deseas guardar los cambios efectuados en \"{fileName}\"?"
        );
    }

    private async void MenuNew_Click(object? sender, RoutedEventArgs e)
    {
        var choice = await PromptUnsavedChangesAsync();
        if (choice == ConfirmChoice.Cancel) return;

        if (choice == ConfirmChoice.Save)
        {
            bool saved = await SaveAsync();
            if (!saved) return;
        }

        _isInitializing = true;
        EditorTextBox.Text = string.Empty;
        _currentFilePath = null;
        _isModified = false;
        _isInitializing = false;

        UpdateTitleAndStatus();
        UpdateCaretPosition();
    }

    private async void MenuOpen_Click(object? sender, RoutedEventArgs e)
    {
        var choice = await PromptUnsavedChangesAsync();
        if (choice == ConfirmChoice.Cancel) return;

        if (choice == ConfirmChoice.Save)
        {
            bool saved = await SaveAsync();
            if (!saved) return;
        }

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Abrir archivo de texto",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Documentos de texto (*.txt)") { Patterns = new[] { "*.txt" } },
                new FilePickerFileType("Todos los archivos (*.*)") { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count >= 1)
        {
            try
            {
                using var stream = await files[0].OpenReadAsync();
                using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
                string content = await reader.ReadToEndAsync();

                _isInitializing = true;
                EditorTextBox.Text = content;
                _currentFilePath = files[0].Path.LocalPath;
                _isModified = false;
                _isInitializing = false;

                UpdateTitleAndStatus();
                UpdateCaretPosition();
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error al abrir", $"No se pudo abrir el archivo:\n{ex.Message}");
            }
        }
    }

    private async void MenuSave_Click(object? sender, RoutedEventArgs e)
    {
        await SaveAsync();
    }

    private async void MenuSaveAs_Click(object? sender, RoutedEventArgs e)
    {
        await SaveAsAsync();
    }

    private async Task<bool> SaveAsync()
    {
        if (string.IsNullOrEmpty(_currentFilePath))
        {
            return await SaveAsAsync();
        }

        try
        {
            await File.WriteAllTextAsync(_currentFilePath, EditorTextBox.Text ?? string.Empty, Encoding.UTF8);
            _isModified = false;
            UpdateTitleAndStatus();
            return true;
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error al guardar", $"No se pudo guardar el archivo:\n{ex.Message}");
            return false;
        }
    }

    private async Task<bool> SaveAsAsync()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return false;

        string suggested = string.IsNullOrEmpty(_currentFilePath) ? "Sin título.txt" : Path.GetFileName(_currentFilePath);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Guardar archivo como",
            SuggestedFileName = suggested,
            DefaultExtension = "txt",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Documentos de texto (*.txt)") { Patterns = new[] { "*.txt" } },
                new FilePickerFileType("Todos los archivos (*.*)") { Patterns = new[] { "*.*" } }
            }
        });

        if (file != null)
        {
            try
            {
                using var stream = await file.OpenWriteAsync();
                using var writer = new StreamWriter(stream, Encoding.UTF8);
                await writer.WriteAsync(EditorTextBox.Text ?? string.Empty);

                _currentFilePath = file.Path.LocalPath;
                _isModified = false;
                UpdateTitleAndStatus();
                return true;
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error al guardar", $"No se pudo guardar el archivo:\n{ex.Message}");
            }
        }
        return false;
    }

    private async void Window_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (!_isModified) return;

        e.Cancel = true;

        var choice = await PromptUnsavedChangesAsync();
        if (choice == ConfirmChoice.Cancel) return;

        if (choice == ConfirmChoice.Save)
        {
            bool saved = await SaveAsync();
            if (!saved) return;
        }

        _isModified = false;
        this.Close();
    }

    private async void MenuExit_Click(object? sender, RoutedEventArgs e)
    {
        var choice = await PromptUnsavedChangesAsync();
        if (choice == ConfirmChoice.Cancel) return;

        if (choice == ConfirmChoice.Save)
        {
            bool saved = await SaveAsync();
            if (!saved) return;
        }

        _isModified = false;
        this.Close();
    }

    private void MenuUndo_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Undo();
    private void MenuRedo_Click(object? sender, RoutedEventArgs e) => EditorTextBox.Redo();
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
            null,
            EditorTextBox.Foreground
        );
        fontWindow.SelectTab(0);

        var result = await fontWindow.ShowDialog<FontColorDialogResult>(this);
        if (result != null)
        {
            ApplyFontColorResult(result);
        }
    }

    private async void MenuColors_Click(object? sender, RoutedEventArgs e)
    {
        var fontWindow = new FontColorWindow(
            EditorTextBox.FontFamily,
            EditorTextBox.FontSize,
            EditorTextBox.FontWeight,
            EditorTextBox.FontStyle,
            null,
            EditorTextBox.Foreground
        );
        fontWindow.SelectTab(1);

        var result = await fontWindow.ShowDialog<FontColorDialogResult>(this);
        if (result != null)
        {
            ApplyFontColorResult(result);
        }
    }

    private void ApplyFontColorResult(FontColorDialogResult result)
    {
        EditorTextBox.FontFamily = result.FontFamily ?? EditorTextBox.FontFamily;
        EditorTextBox.FontSize = result.FontSize;
        _zoomLevel = result.FontSize;
        EditorTextBox.FontWeight = result.FontWeight;
        EditorTextBox.FontStyle = result.FontStyle;

        if (result.ColorChanged && result.Foreground != null)
        {
            EditorTextBox.Foreground = result.Foreground;
        }
        else if (!result.ColorChanged)
        {
            EditorTextBox.ClearValue(TextBox.ForegroundProperty);
        }
    }

    private void MenuZoomIn_Click(object? sender, RoutedEventArgs e)
    {
        _zoomLevel = EditorTextBox.FontSize;
        if (_zoomLevel < 72)
        {
            _zoomLevel += 2;
            EditorTextBox.FontSize = _zoomLevel;
        }
    }

    private void MenuZoomOut_Click(object? sender, RoutedEventArgs e)
    {
        _zoomLevel = EditorTextBox.FontSize;
        if (_zoomLevel > 8)
        {
            _zoomLevel -= 2;
            EditorTextBox.FontSize = _zoomLevel;
        }
    }

    private void MenuZoomReset_Click(object? sender, RoutedEventArgs e)
    {
        _zoomLevel = 14;
        EditorTextBox.FontSize = _zoomLevel;
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
                string tempFile = Path.Combine(Path.GetTempPath(), $"AnimeNotepad_Print_{Guid.NewGuid():N}.txt");
                await File.WriteAllTextAsync(tempFile, textToPrint);
                
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "lpr",
                        Arguments = $"\"{tempFile}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                    await ShowMessageAsync("Impresión", "Documento enviado a la impresora de macOS.");
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
                    await ShowMessageAsync("Impresión", "Documento enviado a la impresora de Windows.");
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
                    await ShowMessageAsync("Impresión", "Documento enviado al sistema de impresión en Linux.");
                }
            }
            catch (Exception ex)
            {
                await ShowMessageAsync("Error de Impresión", $"No se pudo completar la impresión:\n{ex.Message}");
            }
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
                await ShowMessageAsync("Actualización disponible", $"Se ha iniciado la descarga de la nueva versión ({latestVersion}).");
            }
            catch
            {
                await ShowMessageAsync("Actualización disponible", $"Hay una nueva versión disponible ({latestVersion}).\nVisita el repositorio para descargarla.");
            }
        }
        else if (status == AnimeNotepad.Services.UpdateStatus.UpToDate || status == AnimeNotepad.Services.UpdateStatus.Newer)
        {
            await ShowMessageAsync("Actualizaciones", "Ya tienes instalada la versión más reciente.");
        }
        else
        {
            await ShowMessageAsync("Actualizaciones", "No se pudo verificar la actualización en este momento.");
        }
    }

    private Task ShowMessageAsync(string title, string message)
    {
        var tcs = new TaskCompletionSource();
        var msgBox = new Window
        {
            Title = title,
            Width = 360,
            MinHeight = 160,
            SizeToContent = SizeToContent.Height,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var root = new Grid
        {
            Margin = new Thickness(20),
            RowDefinitions = new RowDefinitions("*,Auto")
        };

        var textBlock = new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 20),
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetRow(textBlock, 0);

        var btnOk = new Button
        {
            Content = "Aceptar",
            HorizontalAlignment = HorizontalAlignment.Right,
            Width = 90,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        btnOk.Click += (_, _) => msgBox.Close();
        Grid.SetRow(btnOk, 1);

        root.Children.Add(textBlock);
        root.Children.Add(btnOk);
        msgBox.Content = root;

        msgBox.Closed += (_, _) => tcs.TrySetResult();
        return msgBox.ShowDialog(this);
    }

    private async Task<ConfirmChoice> ShowConfirmDialogAsync(string title, string message)
    {
        var choice = ConfirmChoice.Cancel;
        var dialog = new Window
        {
            Title = title,
            Width = 420,
            MinHeight = 160,
            SizeToContent = SizeToContent.Height,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var root = new Grid
        {
            Margin = new Thickness(20),
            RowDefinitions = new RowDefinitions("*,Auto")
        };

        var textBlock = new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 20),
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 14
        };
        Grid.SetRow(textBlock, 0);

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 10
        };

        var btnSave = new Button
        {
            Content = "Guardar",
            Width = 90,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        btnSave.Click += (_, _) =>
        {
            choice = ConfirmChoice.Save;
            dialog.Close();
        };

        var btnDiscard = new Button
        {
            Content = "No guardar",
            Width = 90,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        btnDiscard.Click += (_, _) =>
        {
            choice = ConfirmChoice.Discard;
            dialog.Close();
        };

        var btnCancel = new Button
        {
            Content = "Cancelar",
            Width = 90,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        btnCancel.Click += (_, _) =>
        {
            choice = ConfirmChoice.Cancel;
            dialog.Close();
        };

        buttonPanel.Children.Add(btnSave);
        buttonPanel.Children.Add(btnDiscard);
        buttonPanel.Children.Add(btnCancel);
        Grid.SetRow(buttonPanel, 1);

        root.Children.Add(textBlock);
        root.Children.Add(buttonPanel);
        dialog.Content = root;

        await dialog.ShowDialog(this);
        return choice;
    }
}