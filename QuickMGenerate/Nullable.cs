using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T?> Nullable<T>(this Generator<State, T> generator)
			where T : struct
		{
			return Nullable(generator, 5);
		}

		public static Generator<State, T?> Nullable<T>(this Generator<State, T> generator, int timesTriedBeforeResultIsNullableAproximation)
			where T : struct
		{
			return
				s =>
				{
					if (s.Random.Next(0, timesTriedBeforeResultIsNullableAproximation) == 0)
						return new Result<State, T?>(null, s);
					var val = generator(s).Value;
					return new Result<State, T?>(val, s);
				};
		}

		public static Generator<State, T?> NeverReturnNull<T>(this Generator<State, T?> generator)
			where T : struct
		{
			return
				s =>
				{
					
					var val = generator(s).Value;
					while(val == null)
						val = generator(s).Value;
					return new Result<State, T?>(val, s);
				};
		}
	}
}