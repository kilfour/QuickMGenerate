using QuickMGenerate.Diagnostics.Inspectors.Calipers;

namespace QuickMGenerate.Diagnostics;

public class WriteDataToFile : IAmAnInspector
{
    private readonly string logFilePath;

    public WriteDataToFile(string? maybePath = null)
    {
        var path = maybePath ?? SolutionLocator.FindSolutionRoot() + "/mgen-log.txt";
        logFilePath = Path.GetFullPath(path);
    }

    public void Log(Entry entry)
    {
        try
        {
            File.AppendAllText(logFilePath,
                entry.Data +
                Environment.NewLine +
                "-------------------------" +
                Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[FileInspector] Failed to log entry: {ex.Message}");
        }
    }
}
