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
    public static async Task<(UpdateStatus status, string? latestVersion)> CheckForUpdatesAsync(string currentVersion)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "AnimeNotepad");
            
            var response = await client.GetAsync("https://api.github.com/repos/RichyKunBv/AnimeNotepad/releases/latest");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("tag_name", out var tagElement))
                {
                    string latestTag = tagElement.GetString() ?? string.Empty;
                    string latestVersionStr = latestTag.TrimStart('v', 'V');
                    
                    if (Version.TryParse(currentVersion, out var curr) && Version.TryParse(latestVersionStr, out var latest))
                    {
                        if (curr < latest) return (UpdateStatus.Outdated, latestVersionStr);
                        if (curr == latest) return (UpdateStatus.UpToDate, latestVersionStr);
                        if (curr > latest) return (UpdateStatus.Newer, latestVersionStr);
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