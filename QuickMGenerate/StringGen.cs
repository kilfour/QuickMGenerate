using System.Text;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, string> String()
		{
			return s =>
			       	{
			       		int numberOfChars = s.Random.Next(1, 10);
			       		var sb = new StringBuilder();
			       		for (int i = 0; i < numberOfChars; i++)
			       		{
			       			sb.Append(Char()(s).Value);
			       		}
			       		return new Result<State, string>(sb.ToString(), s);
			       	};
		}
	}
}