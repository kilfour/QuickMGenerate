using System;
using System.Collections.Generic;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<T> Unique<T>(this Generator<T> generator, object key)
		{
			return
				s =>
					{
						if (!s.GeneratorMemory.ContainsKey(key))
							s.GeneratorMemory[key] = new List<T>();
						var allreadyGenerated = (List<T>) s.GeneratorMemory[key];
						for (int i = 0; i < 50; i++)
						{
							var result = generator(s);
							if(!allreadyGenerated.Contains(result.Value))
							{
								allreadyGenerated.Add(result.Value);
								return result;
							}
						}
						throw new HeyITriedFiftyTimesButCouldNotGetADifferentValue(); 
					};
		}
	}
}