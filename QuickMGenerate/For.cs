﻿using System;
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
			public Generator<State, Unit> Ignore<TProperty>(Expression<Func<T, TProperty>> func)
			{
				return s =>
				{
					s.StuffToIgnore.Add(func.AsPropertyInfo());
					return new Result<State, Unit>(Unit.Instance, s);
				};
			}

			public Generator<State, Unit> Customize<TProperty>(
				Expression<Func<T, TProperty>> func,
				Generator<State, TProperty> propertyGenerator)
			{
				return s =>
				{
					s.Customizations[func.AsPropertyInfo()] = propertyGenerator.AsObject();
					return new Result<State, Unit>(Unit.Instance, s);
				};
			}

			public Generator<State, Unit> Customize<TProperty>(
				Expression<Func<T, TProperty>> func,
				TProperty value)
			{
				return s =>
				{
					s.Customizations[func.AsPropertyInfo()] = Constant(value).AsObject();
					return new Result<State, Unit>(Unit.Instance, s);
				};
			}

			public Generator<State, Unit> Component()
			{
				return s =>
				{
					s.Components.Add(typeof(T));
					return new Result<State, Unit>(Unit.Instance, s);
				};
			}

			public Generator<State, Unit> Apply(Action<T> action)
			{
				return s =>
				{
					s.AddActionToApplyFor(typeof(T), o => action((T)o));
					return new Result<State, Unit>(Unit.Instance, s);
				};
			}
		}
	}
}