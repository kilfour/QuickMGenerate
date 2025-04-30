using System.Linq.Expressions;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public partial class MGen
	{
		public static GeneratorOptions<T> For<T>() where T : class
		{
			return new GeneratorOptions<T>();
		}

		public class GeneratorOptions<T>
		{
			public Generator<Unit> Ignore<TProperty>(Expression<Func<T, TProperty>> func)
			{
				return
					s =>
						{
							s.StuffToIgnore.Add(func.AsPropertyInfo());
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> Customize<TProperty>(
				Expression<Func<T, TProperty>> func,
				Generator<TProperty> propertyGenerator)
			{
				return
					s =>
						{
							s.Customizations[func.AsPropertyInfo()] = propertyGenerator.AsObject();
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> Customize<TProperty>(
				Expression<Func<T, TProperty>> func,
				TProperty value)
			{
				return
					s =>
						{
							s.Customizations[func.AsPropertyInfo()] = Constant(value).AsObject();
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> Component()
			{
				return
					s =>
						{
							s.Components.Add(typeof(T));
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> Component(int maxDepth)
			{

				return
					s =>
						{
							s.RecursionRules[typeof(T)] = (maxDepth, null);
							s.Components.Add(typeof(T));
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> UseThese(params Type[] types)
			{
				return
					s =>
						{
							s.InheritanceInfo[typeof(T)] = types.ToList();
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> Apply(Action<T> action)
			{
				return
					s =>
						{
							s.AddActionToApplyFor(typeof(T), o => action((T)o));
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> Apply<TGen>(Generator<TGen> generator, Action<T, TGen> action)
			{
				return
					s =>
						   {
							   Action<object> objectAction = o => action((T)o, generator(s).Value);
							   s.AddActionToApplyFor(typeof(T), objectAction);
							   return new Result<Unit>(Unit.Instance, s);
						   };
			}
		}
	}
}