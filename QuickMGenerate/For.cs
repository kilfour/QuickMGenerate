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
			public Generator<Unit> IgnoreAll()
			{
				return
					s =>
						{
							s.StuffToIgnoreAll.Add(typeof(T));
							return new Result<Unit>(Unit.Instance, s);
						};
			}

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
			public Generator<Unit> Construct(Func<T> ctor)
			{
				return state =>
				{
					var targetType = typeof(T);
					if (!state.Constructors.TryGetValue(targetType, out var list))
					{
						list = new List<Func<State, object>>();
						state.Constructors[targetType] = list;
					}

					list.Add(s => ctor()!);
					return new Result<Unit>(Unit.Instance, state);
				};
			}

			public Generator<Unit> ConstructFrom(Generator<T> generator)
			{
				return state =>
				{
					Func<State, object> ctorFunc = s =>
					{
						if (s.ConstructionStack.Contains(typeof(T)))
						{
							throw new InvalidOperationException(
								$"Recursive call detected in ConstructFrom for type '{typeof(T)}'. " +
								$"This usually means the generator passed to ConstructFrom indirectly calls MGen.One<{typeof(T).Name}>(), " +
								$"causing infinite recursion.");
						}

						s.ConstructionStack.Push(typeof(T));
						var value = generator(s).Value!;
						s.ConstructionStack.Pop();
						return value;
					};

					if (!state.Constructors.TryGetValue(typeof(T), out var list))
					{
						list = new List<Func<State, object>>();
						state.Constructors[typeof(T)] = list;
					}

					list.Add(ctorFunc);
					return new Result<Unit>(Unit.Instance, state);
				};
			}


			public Generator<Unit> Construct<TArg>(Generator<TArg> generator)
			{
				return state =>
				{
					var targetType = typeof(T);
					var ctor = targetType.GetConstructor([typeof(TArg)]);
					if (ctor == null)
						throw new InvalidOperationException($"No constructor found on {targetType} with argument of type {typeof(TArg)}");

					Func<State, object> ctorFunc = s =>
					{
						var arg = generator(s).Value!;
						return ctor.Invoke([arg]);
					};

					if (!state.Constructors.TryGetValue(targetType, out var list))
					{
						list = new List<Func<State, object>>();
						state.Constructors[targetType] = list;
					}

					list.Add(ctorFunc);
					return new Result<Unit>(Unit.Instance, state);
				};
			}

			public Generator<Unit> Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2)
			{
				return state =>
				{
					var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2)]);
					if (ctor == null)
						throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)})");

					Func<State, object> fn = s =>
					{
						var arg1 = g1(s).Value!;
						var arg2 = g2(s).Value!;
						return ctor.Invoke([arg1, arg2]);
					};

					if (!state.Constructors.TryGetValue(typeof(T), out var list))
						state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

					list.Add(fn);
					return new Result<Unit>(Unit.Instance, state);
				};
			}

			public Generator<Unit> Construct<T1, T2, T3>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3)
			{
				return state =>
				{
					var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2), typeof(T3)]);
					if (ctor == null)
						throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)}, {typeof(T3)})");

					Func<State, object> fn = s =>
					{
						var arg1 = g1(s).Value!;
						var arg2 = g2(s).Value!;
						var arg3 = g3(s).Value!;
						return ctor.Invoke([arg1, arg2, arg3]);
					};

					if (!state.Constructors.TryGetValue(typeof(T), out var list))
						state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

					list.Add(fn);
					return new Result<Unit>(Unit.Instance, state);
				};
			}

			public Generator<Unit> Construct<T1, T2, T3, T4>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4)
			{
				return state =>
				{
					var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2), typeof(T3), typeof(T4)]);
					if (ctor == null)
						throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)}, {typeof(T3)}, {typeof(T4)})");

					Func<State, object> fn = s =>
					{
						var arg1 = g1(s).Value!;
						var arg2 = g2(s).Value!;
						var arg3 = g3(s).Value!;
						var arg4 = g4(s).Value!;
						return ctor.Invoke([arg1, arg2, arg3, arg4]);
					};

					if (!state.Constructors.TryGetValue(typeof(T), out var list))
						state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

					list.Add(fn);
					return new Result<Unit>(Unit.Instance, state);
				};
			}

			public Generator<Unit> Construct<T1, T2, T3, T4, T5>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4, Generator<T5> g5)
			{
				return state =>
				{
					var ctor = typeof(T).GetConstructor([typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)]);
					if (ctor == null)
						throw new InvalidOperationException($"No constructor found on {typeof(T)} with args ({typeof(T1)}, {typeof(T2)}, {typeof(T3)}, {typeof(T4)}, {typeof(T5)})");

					Func<State, object> fn = s =>
					{
						var arg1 = g1(s).Value!;
						var arg2 = g2(s).Value!;
						var arg3 = g3(s).Value!;
						var arg4 = g4(s).Value!;
						var arg5 = g5(s).Value!;
						return ctor.Invoke([arg1, arg2, arg3, arg4, arg5]);
					};

					if (!state.Constructors.TryGetValue(typeof(T), out var list))
						state.Constructors[typeof(T)] = list = new List<Func<State, object>>();

					list.Add(fn);
					return new Result<Unit>(Unit.Instance, state);
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
							s.AddActionToApplyFor(typeof(T), (s, o) => action((T)o));
							return new Result<Unit>(Unit.Instance, s);
						};
			}

			public Generator<Unit> Apply<TGen>(Generator<TGen> generator, Action<T, TGen> action)
			{
				return
					s =>
						   {
							   Action<State, object> objectAction = (s1, o) => action((T)o, generator(s1).Value);
							   s.AddActionToApplyFor(typeof(T), objectAction);
							   return new Result<Unit>(Unit.Instance, s);
						   };
			}
		}
	}
}