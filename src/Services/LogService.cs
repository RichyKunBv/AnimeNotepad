using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AnimeNotepad.Services;

public static class LogService
{
    private static readonly object Sync = new();

    public static event Action<string>? MessageLogged;

    public static string LogDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "AnimeNotepad",
        "logs");

    public static void Info(string source, string message) => Write("INFO", source, message, null);
    public static void Warn(string source, string message, Exception? exception = null) => Write("WARN", source, message, exception);
    public static void Error(string source, string message, Exception? exception = null) => Write("ERROR", source, message, exception);

    public static string[] GetRecentLogs()
    {
        try
        {
            if (!Directory.Exists(LogDirectory)) return Array.Empty<string>();
            return Directory.GetFiles(LogDirectory, "*.log")
                .OrderBy(path => path, StringComparer.Ordinal)
                .SelectMany(File.ReadAllLines)
                .TakeLast(5000)
                .ToArray();
        }
        catch (Exception ex)
        {
            return new[] { $"No se pudieron leer los registros: {ex.Message}" };
        }
    }

    public static void Clear()
    {
        try
        {
            if (!Directory.Exists(LogDirectory)) return;
            foreach (string path in Directory.GetFiles(LogDirectory, "*.log"))
                File.Delete(path);
        }
        catch (Exception ex)
        {
            Error("LogService", "No se pudieron limpiar los registros", ex);
        }
    }

    private static void Write(string level, string source, string message, Exception? exception)
    {
        string line = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz} [{level}] [{source}] {message}";
        if (exception != null) line += Environment.NewLine + exception;

        try
        {
            lock (Sync)
            {
                Directory.CreateDirectory(LogDirectory);
                File.AppendAllText(
                    Path.Combine(LogDirectory, $"animenotepad-{DateTime.Now:yyyyMMdd}.log"),
                    line + Environment.NewLine,
                    Encoding.UTF8);
            }
        }
        catch
        {
        }

        MessageLogged?.Invoke(line);
    }
}