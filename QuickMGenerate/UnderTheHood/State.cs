using System;
using System.Collections.Generic;
using System.Reflection;

namespace QuickMGenerate.UnderTheHood
{
	public class State
	{
		public readonly Random Random = new Random();

		public readonly List<PropertyInfo> StuffToIgnore = new List<PropertyInfo>();

		public readonly Dictionary<Type, Generator<State, object>> PrimitiveGenerators
			= new Dictionary<Type, Generator<State, object>>
			  	{
			  		{ typeof(int), MGen.Int().AsObject() }
				};
	}
}