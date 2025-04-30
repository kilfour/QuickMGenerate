using System.Reflection;

namespace QuickMGenerate.UnderTheHood
{
	public class State
	{
		public readonly MoreRandom Random = new MoreRandom();

		public readonly List<PropertyInfo> StuffToIgnore = new List<PropertyInfo>();

		private readonly Dictionary<object, object> generatorMemory =
			new Dictionary<object, object>();

		public T Get<T>(object key, T newValue)
		{
			if (!generatorMemory.ContainsKey(key))
				generatorMemory[key] = newValue!;
			return (T)generatorMemory[key];
		}

		public void Set<T>(object key, T value)
		{
			generatorMemory[key] = value!;
		}

		public readonly List<Type> Components = new List<Type>();

		public readonly Dictionary<Type, List<Type>> InheritanceInfo
			= new Dictionary<Type, List<Type>>();

		public Dictionary<Type, (int MaxDepth, Type? FallbackType)> RecursionRules = new();

		public readonly Dictionary<PropertyInfo, Generator<object>> Customizations
			= new Dictionary<PropertyInfo, Generator<object>>();

		public readonly Dictionary<Type, List<Func<State, object>>> Constructors
			= new Dictionary<Type, List<Func<State, object>>>();

		public readonly Dictionary<Type, List<Action<object>>> ActionsToApply =
			new Dictionary<Type, List<Action<object>>>();

		public void AddActionToApplyFor(Type type, Action<object> action)
		{
			if (!ActionsToApply.ContainsKey(type))
				ActionsToApply[type] = new List<Action<object>>();
			var actions = ActionsToApply[type];
			if (actions.All(a => a.GetHashCode() != action.GetHashCode()))
				actions.Add(action);
		}

		public readonly Dictionary<Type, Generator<object>> PrimitiveGenerators
			= new Dictionary<Type, Generator<object>>
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