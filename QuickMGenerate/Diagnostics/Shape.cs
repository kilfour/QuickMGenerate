namespace QuickMGenerate.Diagnostics;

public static class Shape
{

    public static Shaper Entry(IAmAnArtist reshaper)
    {
        return new Shaper(reshaper);
    }

    public static Shaper Data(Func<object, object> sculptData)
    {
        return new Shaper(new GenericSculptor(sculptData));
    }

    public class Shaper
    {
        private IAmAnArtist reshaper;
        public Shaper(IAmAnArtist reshaper) { this.reshaper = reshaper; }
        public Tap For(IAmAnInspector inspector)
        {
            return new Tap(inspector, reshaper);
        }
    }

    public class Tap : IAmAnInspector
    {
        private readonly IAmAnInspector next;
        private IAmAnArtist? reshaper;

        public Tap(IAmAnInspector next, IAmAnArtist? reshaper)
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
}




