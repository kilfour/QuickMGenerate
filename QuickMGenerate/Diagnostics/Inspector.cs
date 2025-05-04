namespace QuickMGenerate.Diagnostics;


public record Entry(string[] Tags, string Message, object Data);
public interface IAmAnInspector { void Log(Entry entry); }
public interface IReshaper { Entry Sculpt(Entry entry); }

public class ThePlumber
{
    private readonly IAmAnInspector inspector;
    private IReshaper? reshaper;

    public ThePlumber(IAmAnInspector inspector)
    {
        this.inspector = inspector;
    }

    public ThePlumber Reshape(IReshaper reshaper)
    {
        this.reshaper = reshaper;
        return this;
    }

    public IAmAnInspector To(IAmAnInspector next)
    {
        return new PipeLine(inspector, next, reshaper);
    }
}

public static class PipeIt
{
    public static ThePlumber From(IAmAnInspector inspector)
    {
        return new ThePlumber(inspector);
    }
}

public static class Shape
{
    public static Shaper Entry(IReshaper reshaper)
    {
        return new Shaper(reshaper);
    }
}

public class Shaper
{
    private IReshaper reshaper;
    public Shaper(IReshaper reshaper) { this.reshaper = reshaper; }
    public Tap For(IAmAnInspector inspector)
    {
        return new Tap(inspector, reshaper);
    }
}

public class Tap : IAmAnInspector
{
    private readonly IAmAnInspector next;
    private IReshaper? reshaper;

    public Tap(IAmAnInspector next, IReshaper? reshaper)
    {
        this.next = next;
        this.reshaper = reshaper;
    }

    public void Log(Entry entry)
    {
        next.Log(Sculpt(entry));
    }

    private Entry Sculpt(Entry entry)
    {
        return reshaper == null ? entry : reshaper.Sculpt(entry);
    }
}

public class PipeLine : IAmAnInspector
{
    private readonly IAmAnInspector inspector;
    private readonly IAmAnInspector next;
    private IReshaper? reshaper;

    public PipeLine(IAmAnInspector inspector, IAmAnInspector next, IReshaper? reshaper)
    {
        this.inspector = inspector;
        this.next = next;
        this.reshaper = reshaper;
    }

    public void Log(Entry entry)
    {
        inspector.Log(entry);
        next.Log(Sculpt(entry));
    }

    private Entry Sculpt(Entry entry)
    {
        return reshaper == null ? entry : reshaper.Sculpt(entry);
    }
}
