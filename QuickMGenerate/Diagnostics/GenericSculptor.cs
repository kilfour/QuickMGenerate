namespace QuickMGenerate.Diagnostics;

public class GenericSculptor : IAmAnArtist
{
    private readonly Func<object, object> sculptData;
    public GenericSculptor(Func<object, object> sculptData) { this.sculptData = sculptData; }
    public Entry Sculpt(Entry entry)
    {
        return new Entry(entry.Tags, entry.Message, sculptData(entry.Data));
    }
}




