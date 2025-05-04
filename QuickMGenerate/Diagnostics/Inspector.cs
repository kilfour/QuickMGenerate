namespace QuickMGenerate.Diagnostics;

public interface Inspector
{
    void Log(string[] tags, string message, object structuredData);
}