using Azure.Core;
using System.Net.Mime;

namespace FitnessAppAPI.Services.Helpers;

public static class FilesHandler
{
    public static async Task<string> SaveFile(IFormFile file)
    {
        if (!Directory.Exists("Files"))
        {
            Directory.CreateDirectory("Files");
        }

        var ext = file.ContentType.Contains("image") ? ".jpg" : ".mp4";

        var guid = Guid.NewGuid().ToString().Replace("-", "").Replace("$", "");
        string filePath = $"{guid}{ext}";
        using Stream fileStream = new FileStream($"Files/{filePath}", FileMode.Create);
        await file.CopyToAsync(fileStream);

        return Task.FromResult(filePath).Result;
    }
}
