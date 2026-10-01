using System;
using System.IO;
using System.Text.Json;

namespace AnimeNotepad.Services;

public sealed class EditorSettings
{
    public string? FontFamily { get; set; }
    public double FontSize { get; set; } = 14;
    public bool IsBold { get; set; }
    public bool IsItalic { get; set; }
    public string? Foreground { get; set; }
}

public static class SettingsService
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "AnimeNotepad",
        "settings.json");

    public static EditorSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsPath)) return new EditorSettings();
            return JsonSerializer.Deserialize<EditorSettings>(File.ReadAllText(SettingsPath)) ?? new EditorSettings();
        }
        catch (Exception ex)
        {
            LogService.Warn("SettingsService", "No se pudieron cargar las preferencias; se usarán los valores predeterminados", ex);
            return new EditorSettings();
        }
    }

    public static void Save(EditorSettings settings)
    {
        try
        {
            string directory = Path.GetDirectoryName(SettingsPath)!;
            Directory.CreateDirectory(directory);
            string temporaryPath = SettingsPath + ".tmp";
            File.WriteAllText(temporaryPath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temporaryPath, SettingsPath, true);
        }
        catch (Exception ex)
        {
            LogService.Warn("SettingsService", "No se pudieron guardar las preferencias", ex);
        }
    }
}