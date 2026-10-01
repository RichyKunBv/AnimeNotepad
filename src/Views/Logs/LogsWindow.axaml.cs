using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using AnimeNotepad.Services;

namespace AnimeNotepad.Views.Logs;

public partial class LogsWindow : Window
{
    private string[] _allLines = Array.Empty<string>();

    public LogsWindow()
    {
        InitializeComponent();
        _allLines = LogService.GetRecentLogs();
        ApplyFilter();
        LogService.MessageLogged += OnMessageLogged;
    }

    private void OnMessageLogged(string line)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            _allLines = _allLines.Append(line).TakeLast(5000).ToArray();
            ApplyFilter();
        });
    }

    private void ApplyFilter()
    {
        string filter = FilterBox.Text?.Trim() ?? string.Empty;
        string[] matching = string.IsNullOrEmpty(filter)
            ? _allLines
            : _allLines.Where(line => line.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToArray();
        LogsTextBox.Text = string.Join(Environment.NewLine, matching);
        StatsText.Text = $"{matching.Length} de {_allLines.Length} líneas";
    }

    private void Filter_Changed(object? sender, TextChangedEventArgs e) => ApplyFilter();

    private async void Copy_Click(object? sender, RoutedEventArgs e)
    {
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard == null || string.IsNullOrEmpty(LogsTextBox.Text)) return;
        await clipboard.SetTextAsync(LogsTextBox.Text);
        StatsText.Text = "Registros copiados";
    }

    private void OpenFolder_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            Directory.CreateDirectory(LogService.LogDirectory);
            OpenWithSystem(LogService.LogDirectory);
        }
        catch (Exception ex)
        {
            LogService.Error("LogsWindow", "No se pudo abrir la carpeta de registros", ex);
        }
    }

    private void Clear_Click(object? sender, RoutedEventArgs e)
    {
        LogService.Clear();
        _allLines = Array.Empty<string>();
        ApplyFilter();
    }

    private void ReportGitHub_Click(object? sender, RoutedEventArgs e)
    {
        string diagnostics = BuildDiagnostics();
        string body = $"### Descripción del problema\n\n<!-- Describe qué ocurrió y cómo reproducirlo -->\n\n### Diagnóstico\n```text\n{diagnostics}\n```";
        OpenReportUrl($"https://github.com/RichyKunBv/AnimeNotepad/issues/new?title={WebUtility.UrlEncode("[Error] AnimeNotepad")}&body={WebUtility.UrlEncode(body)}");
    }

    private void ReportEmail_Click(object? sender, RoutedEventArgs e)
    {
        string body = $"Describe el problema y cómo reproducirlo aquí:\n\n{BuildDiagnostics()}";
        string url = $"mailto:esmesolutions0@gmail.com?subject={Uri.EscapeDataString($"Reporte de error AnimeNotepad {Verzion.Texto}")}&body={Uri.EscapeDataString(body)}";
        OpenReportUrl(url);
    }

    private string BuildDiagnostics()
    {
        string system = $"Sistema: {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})\n" +
                        $"Versión: {Verzion.Texto}\n.NET: {Environment.Version}";
        string logs = string.Join(Environment.NewLine, _allLines.TakeLast(30));
        return $"{system}\n\nRegistros recientes:\n{logs}";
    }

    private void OpenReportUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            LogService.Error("LogsWindow", "No se pudo abrir el destino del reporte", ex);
        }
    }

    private static void OpenWithSystem(string path)
    {
        string command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "explorer.exe" :
                         RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "open" : "xdg-open";
        Process.Start(new ProcessStartInfo(command, path) { UseShellExecute = false });
    }

    private void Close_Click(object? sender, RoutedEventArgs e) => Close();

    protected override void OnClosed(EventArgs e)
    {
        LogService.MessageLogged -= OnMessageLogged;
        base.OnClosed(e);
    }
}