namespace QuickMGenerate.Diagnostics.Inspectors.Calipers;

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