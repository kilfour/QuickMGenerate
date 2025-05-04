namespace QuickMGenerate.Diagnostics.Inspectors;

public class CompositeInspector : Inspector
{
    private readonly List<Inspector> _inspectors = new();

    public CompositeInspector(params Inspector[] inspectors)
    {
        _inspectors.AddRange(inspectors);
    }

    public void Add(Inspector inspector) => _inspectors.Add(inspector);

    public void Log(string[] tags, string message, object structuredData)
    {
        foreach (var inspector in _inspectors)
            inspector.Log(tags, message, structuredData);
    }
}