using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, string> Append(this Generator<State, string> generator, string appendix)
		{
			return
				s =>
					{  
						var val = string.Format("{0}{1}", generator(s).Value, appendix);
						return new Result<State, string>(val, s);
					};
		}

		public static Generator<State, string> Append(this Generator<State, string> generator, Generator<State, string> appendix)
		{
			return
				s =>
				{
					var val = string.Format("{0}{1}", generator(s).Value, appendix(s).Value);
					return new Result<State, string>(val, s);
				};
		}
	}
}