using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnimeNotepad.Services;

public enum UpdateStatus
{
    Error,
    Outdated,
    UpToDate,
    Newer
}

public static class UpdateService
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    static UpdateService()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "AnimeNotepad");
    }

    public static async Task<(UpdateStatus status, string? latestVersion)> CheckForUpdatesAsync(string currentVersion)
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.github.com/repos/RichyKunBv/AnimeNotepad/releases/latest");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("tag_name", out var tagElement))
                {
                    string latestTag = tagElement.GetString() ?? string.Empty;
                    string cleanLatest = CleanVersionString(latestTag);
                    string cleanCurrent = CleanVersionString(currentVersion);

                    if (Version.TryParse(cleanCurrent, out var curr) && Version.TryParse(cleanLatest, out var latest))
                    {
                        if (curr < latest) return (UpdateStatus.Outdated, latestTag);
                        if (curr == latest) return (UpdateStatus.UpToDate, latestTag);
                        if (curr > latest) return (UpdateStatus.Newer, latestTag);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[UpdateChecker] Error checking for updates: {ex.Message}");
        }
        return (UpdateStatus.Error, null);
    }

    private static string CleanVersionString(string rawVersion)
    {
        if (string.IsNullOrWhiteSpace(rawVersion)) return "0.0.0.0";
        var trimmed = rawVersion.Trim().TrimStart('v', 'V');
        var match = System.Text.RegularExpressions.Regex.Match(trimmed, @"^[0-9]+(\.[0-9]+)+");
        return match.Success ? match.Value : trimmed;
    }

    public static string GetDirectDownloadUrl()
    {
        string baseUrl = "https://github.com/RichyKunBv/AnimeNotepad/releases/latest/download/";
        string fileName = "AnimeNotepad";

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
        {
            if (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.Arm64)
                fileName = "AnimeNotepad-arm64.dmg";
            else
                fileName = "AnimeNotepad-x64.dmg";
        }
        else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            if (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.Arm64)
                fileName = "AnimeNotepad-arm64.exe"; 
            else
                fileName = "AnimeNotepad-x64.exe"; 
        }
        else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            if (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.Arm64)
                fileName = "AnimeNotepad-arm64.AppImage";
            else
                fileName = "AnimeNotepad-x64.AppImage";
        }

        return baseUrl + fileName;
    }
}