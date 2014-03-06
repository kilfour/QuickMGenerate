using System.Collections.Generic;
using QuickMGenerate.UnderTheHood;

namespace QuickMGenerate
{
	public static partial class MGen
	{
		public static Generator<State, T> Unique<T>(this Generator<State, T> generator)
		{
			return
				s =>
					{
						if (!s.GeneratorMemory.ContainsKey(generator))
							s.GeneratorMemory[generator] = new List<T>();
						var allreadyGenerated = (List<T>) s.GeneratorMemory[generator];
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