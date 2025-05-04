using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickMGenerate.Diagnostics;

public class AppendToWebviewInspector : IAmAnInspector
{
    private readonly string logFilePath;
    public static class SolutionLocator
    {
        public static string? FindSolutionRoot(string? startDirectory = null)
        {
            var dir = new DirectoryInfo(startDirectory ?? Directory.GetCurrentDirectory());
            while (dir != null)
            {
                if (dir.GetFiles("*.sln").Length != 0)
                    return dir.FullName;

                dir = dir.Parent;
            }
            return null;
        }
    }

    public AppendToWebviewInspector(string? maybePath = null)
    {
        var path = maybePath ?? SolutionLocator.FindSolutionRoot() + "/pbt-inspector.ndjson";
        logFilePath = Path.GetFullPath(path);
    }

    public void Log(Entry entry)
    {
        try
        {
            string json =
                JsonSerializer.Serialize(entry, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    PropertyNamingPolicy = new LowercaseFirstLetterPolicy(),
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

public class LowercaseFirstLetterPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
            return name;

        return char.ToLower(name[0]) + name.Substring(1);
    }
}