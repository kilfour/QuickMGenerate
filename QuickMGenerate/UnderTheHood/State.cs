using System.Reflection;
using System.Security.Cryptography;

namespace QuickMGenerate.UnderTheHood
{
	public record DepthConstraint(int Min, int Max);

	public record DepthFrame(Type Type, int Depth);

	public class State
	{
		public int Seed { get; }
		public MoreRandom Random { get; }

		public State()
		{
			Seed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
			Random = new MoreRandom(Seed);
		}

		public State(int seed)
		{
			Seed = seed;
			Random = new MoreRandom(seed);
		}

		// ---------------------------------------------------------------------
		// Depth Control
		public readonly Dictionary<Type, DepthConstraint> DepthConstraints = [];

		private readonly Stack<DepthFrame> depthFrames = new();

		public DepthConstraint GetDepthConstraint(Type type) =>
			DepthConstraints.TryGetValue(type, out var c) ? c : new(1, 1);

		public int GetDepth(Type type) =>
			depthFrames.FirstOrDefault(f => f.Type == type)?.Depth ?? 0;

		public void PushDepthFrame(Type type)
			=> depthFrames.Push(new(type, GetDepth(type) + 1));

		public void PopDepthFrame() => depthFrames.Pop();

		public DisposableAction WithDepthFrame(Type type)
		{
			PushDepthFrame(type);
			return new DisposableAction(PopDepthFrame);
		}
		// ---------------------------------------------------------------------

		public readonly List<Type> StuffToIgnoreAll = new List<Type>();

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

		public readonly Dictionary<Type, List<Type>> InheritanceInfo
			= new Dictionary<Type, List<Type>>();

		public Dictionary<Type, Type> TreeLeaves = new();

		public readonly Dictionary<PropertyInfo, Generator<object>> Customizations
			= new Dictionary<PropertyInfo, Generator<object>>();

		public readonly Dictionary<Type, List<Func<State, object>>> Constructors
			= new Dictionary<Type, List<Func<State, object>>>();

		public readonly Dictionary<Type, List<Action<State, object>>> ActionsToApply =
			new Dictionary<Type, List<Action<State, object>>>();

		// --- MGENCHOOSEFROM --- public Stack<Type> ConstructionStack { get; } = new();

		public void AddActionToApplyFor(Type type, Action<State, object> action)
		{
			if (!ActionsToApply.ContainsKey(type))
				ActionsToApply[type] = new List<Action<State, object>>();
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