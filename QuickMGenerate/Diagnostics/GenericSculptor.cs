namespace QuickMGenerate.Diagnostics;

public class GenericSculptor : IAmAnArtist
{
    private readonly Func<Entry, Entry> chisel;
    public GenericSculptor(Func<Entry, Entry> chisel) { this.chisel = chisel; }
    public Entry Sculpt(Entry entry)
    {
        return chisel(entry);
    }
}




