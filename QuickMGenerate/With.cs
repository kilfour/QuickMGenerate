using System;
using System.Linq.Expressions;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public partial class MGen
	{
		public static GeneratorOptions<T> With<T>()
		{
			return new GeneratorOptions<T>();
		}
	}

	public class GeneratorOptions<T>
	{
		public Generator<State, Unit> Ignore<TProperty>(Expression<Func<T, TProperty>> func)
		{
			return s =>
			       	{
			       		s.StuffToIgnore.Add(func.AsPropertyInfo());
			       		return new Result<State, Unit>(Unit.Instance, s);
			       	};
		}
	}
}