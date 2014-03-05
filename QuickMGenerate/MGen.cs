using System;
using System.Collections.Generic;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static T Generate<T>(this Generator<State, T> generator)
		{
			return generator(new State()).Value;
		}

		public static T Generate<T>(this Generator<State, T> generator, State state)
		{
			return generator(state).Value;
		}

		public static Generator<State, IEnumerable<T>> Many<T>(this Generator<State, T> generator, int number)
		{
			return
				s =>
					{
						var list = new T[number];
						for (int i = 0; i < number; i++)
						{
							list[i] = generator(s).Value;
						}
						return new Result<State, IEnumerable<T>>(list, s);
					};

		}

		

		public static Generator<State, T> One<T>()
		{
			return s => new Result<State, T>(Activator.CreateInstance<T>(), s);
		}

		public static Generator<State, T> Do<T>(Action<T> action)
		{
			var t = default(T);
			action(t);
			return s => new Result<State, T>(t, s);
		}

		public static Generator<State, T> Do<T>(T value, Action<T> action)
		{
			action(value);
			return s => new Result<State, T>(value, s);
		}
	}
}