namespace QuickMGenerate.Diagnostics;

public class GenericDataSculptor : IAmAnArtist
{
    private readonly Func<object, object> sculptData;
    public GenericDataSculptor(Func<object, object> sculptData) { this.sculptData = sculptData; }
    public Entry Sculpt(Entry entry)
    {
        return new Entry(entry.Tags, entry.Message, sculptData(entry.Data));
    }
}




