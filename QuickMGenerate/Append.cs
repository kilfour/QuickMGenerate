using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<string> Append(this Generator<string> generator, string appendix)
		{
			return
				s =>
					{  
						var val = string.Format("{0}{1}", generator(s).Value, appendix);
						return new Result<string>(val, s);
					};
		}

		public static Generator<string> Append(this Generator<string> generator, Generator<string> appendix)
		{
			return
				s =>
				{
					var val = string.Format("{0}{1}", generator(s).Value, appendix(s).Value);
					return new Result<string>(val, s);
				};
		}
	}
}