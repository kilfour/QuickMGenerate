using System;
using System.Linq.Expressions;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public partial class MGen
	{
		public static Generator<State, Unit> Ignore<T, TProperty>(this Generator<State, T> generator, Expression<Func<T, TProperty>> func)
		{
			return s =>
			{
				s.StuffToIgnore.Add(func.AsPropertyInfo());
				return new Result<State, Unit>(Unit.Instance, s);
			};
		}
	}
}