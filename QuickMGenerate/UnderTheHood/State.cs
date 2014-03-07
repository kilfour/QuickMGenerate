using System;
using System.Collections.Generic;
using System.Reflection;

namespace QuickMGenerate.UnderTheHood
{
	public class State
	{
		public readonly Random Random = new Random();

		public readonly List<PropertyInfo> StuffToIgnore = new List<PropertyInfo>();

		public readonly Dictionary<object, object> GeneratorMemory =
			new Dictionary<object, object>();

		public readonly List<Type> Components = new List<Type>();

		public readonly Dictionary<PropertyInfo, Generator<State, object>> Customizations
			= new Dictionary<PropertyInfo, Generator<State, object>>();

		public readonly Dictionary<Type, List<Action<object>>> ActionsToApply =
			new Dictionary<Type, List<Action<object>>>();

		public void AddActionToApplyFor(Type type, Action<object> action)
		{
			if(!ActionsToApply.ContainsKey(type))
				ActionsToApply[type] = new List<Action<object>>();
			var actions = ActionsToApply[type];
			actions.Add(action);
		}

		public readonly Dictionary<Type, Generator<State, object>> PrimitiveGenerators
			= new Dictionary<Type, Generator<State, object>>
			  	{
					{ typeof(string), MGen.String().AsObject() },
			  		{ typeof(int), MGen.Int().AsObject() },
					{ typeof(int?), MGen.Int().Nullable().AsObject() },
					{ typeof(char), MGen.Char().AsObject() },
					{ typeof(char?), MGen.Char().Nullable().AsObject() },
					{ typeof(bool), MGen.Bool().AsObject() },
					{ typeof(bool?), MGen.Bool().Nullable().AsObject() },
					{ typeof(decimal), MGen.Decimal().AsObject() },
					{ typeof(decimal?), MGen.Decimal().Nullable().AsObject() },
					{ typeof(DateTime), MGen.DateTime().AsObject() },
					{ typeof(DateTime?), MGen.DateTime().Nullable().AsObject() },
					{ typeof(long), MGen.Long().AsObject() },
					{ typeof(long?), MGen.Long().Nullable().AsObject() },
					{ typeof(double), MGen.Double().AsObject() },
					{ typeof(double?), MGen.Double().Nullable().AsObject() },
					{ typeof(float), MGen.Float().AsObject() },
					{ typeof(float?), MGen.Float().Nullable().AsObject() },
					{ typeof(Guid), MGen.Guid().AsObject() },
					{ typeof(Guid?), MGen.Guid().Nullable().AsObject() },
					{ typeof(short), MGen.Short().AsObject() },
					{ typeof(short?), MGen.Short().Nullable().AsObject() },
					{ typeof(TimeSpan), MGen.TimeSpan().AsObject() },
					{ typeof(TimeSpan?), MGen.TimeSpan().Nullable().AsObject() }
				};
	}
}