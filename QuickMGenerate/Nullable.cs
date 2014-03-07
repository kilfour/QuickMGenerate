using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T?> Nullable<T>(this Generator<T> generator)
			where T : struct
		{
			return Nullable(generator, 5);
		}

		public static Generator<T?> Nullable<T>(this Generator<T> generator, int timesTriedBeforeResultIsNullableAproximation)
			where T : struct
		{
			return
				s =>
				{
					if (s.Random.Next(0, timesTriedBeforeResultIsNullableAproximation) == 0)
						return new Result<T?>(null, s);
					var val = generator(s).Value;
					return new Result<T?>(val, s);
				};
		}

		public static Generator<T?> NeverReturnNull<T>(this Generator<T?> generator)
			where T : struct
		{
			return
				s =>
				{
					
					var val = generator(s).Value;
					while(val == null)
						val = generator(s).Value;
					return new Result<T?>(val, s);
				};
		}
	}
}