namespace QuickMGenerate.UnderTheHood.Diagnostics;

public interface Inspector
{
    void Log(string[] tags, string message, object structuredData);
}
