namespace QuickMGenerate
{
	public class HeyITriedFiftyTimesButCouldNotGetADifferentValue
		: Exception
	{
		public HeyITriedFiftyTimesButCouldNotGetADifferentValue() { }

		public HeyITriedFiftyTimesButCouldNotGetADifferentValue(string message)
			: base(message) { }
	}
}