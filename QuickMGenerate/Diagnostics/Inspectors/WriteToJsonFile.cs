using System.Text.Json;
using System.Text.Json.Serialization;
using QuickMGenerate.Diagnostics.Inspectors.Calipers;

namespace QuickMGenerate.Diagnostics;

public class WriteToJsonFile : IAmAnInspector
{
    private readonly string logFilePath;

    public WriteToJsonFile(string? maybePath = null)
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
    public class LowercaseFirstLetterPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
                return name;

            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}

