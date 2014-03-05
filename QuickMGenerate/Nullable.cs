﻿using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T?> Nullable<T>(this Generator<State, T> generator)
			where T : struct
		{
			return
				s =>
					{  
						if(s.Random.Next(0, 5) == 0)
							return new Result<State, T?>(null, s);
						var val = generator(s).Value;
						return new Result<State, T?>(val, s);
					};
		}
	}
}