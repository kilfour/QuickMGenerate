namespace QuickMGenerate.Diagnostics;

public static class PipeIt
{
    public static ThePlumber From(IAmAnInspector inspector)
    {
        return new ThePlumber(inspector);
    }

    public class ThePlumber
    {
        private readonly IAmAnInspector inspector;
        private IAmAnArtist? reshaper;

        public ThePlumber(IAmAnInspector inspector)
        {
            this.inspector = inspector;
        }

        public ThePlumber Reshape(IAmAnArtist reshaper)
        {
            this.reshaper = reshaper;
            return this;
        }

        public IAmAnInspector To(IAmAnInspector next)
        {
            return new PipeLine(inspector, next, reshaper);
        }
    }

    public class PipeLine : IAmAnInspector
    {
        private readonly IAmAnInspector inspector;
        private readonly IAmAnInspector next;
        private IAmAnArtist? reshaper;

        public PipeLine(IAmAnInspector inspector, IAmAnInspector next, IAmAnArtist? reshaper)
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
}




