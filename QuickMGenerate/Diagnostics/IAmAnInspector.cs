namespace QuickMGenerate.Diagnostics;

public interface IAmAnInspector { void Log(Entry entry); }


public static class InspectorExtensions
{
    public static IAmAnInspector ReshapeData<T, U>(this IAmAnInspector inspector, Func<T, U> chisel)
    {
        return new GenericInspector(inspector, new GenericDataSculptor(b => chisel((T)b)!));
    }
}

public class GenericInspector : IAmAnInspector
{
    private readonly IAmAnInspector inspector;
    private readonly IAmAnArtist artist;

    public GenericInspector(IAmAnInspector inspector, IAmAnArtist artist)
    {
        this.inspector = inspector;
        this.artist = artist;
    }

    public void Log(Entry entry)
    {
        inspector.Log(artist.Sculpt(entry));
    }
}




