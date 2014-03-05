using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, char> Char()
		{
			const int lowerCaseLetterACode = 97;
			const int lowerCaseLetterZCode = 122;
			return s => new Result<State, char>((char)s.Random.Next(lowerCaseLetterACode, lowerCaseLetterZCode + 1), s);
		}
	}
}