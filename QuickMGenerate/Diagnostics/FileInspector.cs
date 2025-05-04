using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickMGenerate.Diagnostics;

public class FileInspector : Inspector
{
    private readonly string logFilePath;
    public static class SolutionLocator
    {
        public static string? FindSolutionRoot(string? startDirectory = null)
        {
            var dir = new DirectoryInfo(startDirectory ?? Directory.GetCurrentDirectory());

            while (dir != null)
            {
                if (dir.GetFiles("*.sln").Any())
                    return dir.FullName;

                dir = dir.Parent;
            }

            return null;
        }
    }
    public FileInspector(string? maybePath = null)
    {
        var path = maybePath ?? SolutionLocator.FindSolutionRoot() + "/pbt-inspector.ndjson";
        logFilePath = Path.GetFullPath(path);
    }

    public void Log(string[] tags, string message, object structuredData)
    {
        try
        {
            var logEntry = new
            {
                tags,
                message,
                data = structuredData
            };

            string json = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            File.AppendAllText(logFilePath, json + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[FileInspector] Failed to log entry: {ex.Message}");
        }
    }
}