using System;
using System.IO;
using System.Linq;
using System.Text;

namespace QuickMGenerate.Tests.Tools
{
	public class CreateDoc
	{
		public void Go()
		{
			var typeattributes =
				typeof (CreateDoc).Assembly
					.GetTypes()
					.SelectMany(t => t.GetCustomAttributes(typeof (DocAttribute), false));

			var methodattributes =
				typeof (CreateDoc).Assembly
					.GetTypes()
					.SelectMany(t => t.GetMethods())
					.SelectMany(t => t.GetCustomAttributes(typeof (DocAttribute), false));

			var attributes = typeattributes.Union(methodattributes)
					.Cast<DocAttribute>();

			var chapters = attributes.OrderBy(a => a.ChapterOrder).Select(a => a.Chapter).Distinct();
			var sb = new StringBuilder();
			sb.AppendLine(Introduction);
			foreach (var chapter in chapters)
			{
				sb.AppendFormat("##{0}", chapter);
				sb.AppendLine();
				var chapterAttributes = attributes.Where(a => a.Chapter == chapter);
				var captions = chapterAttributes.OrderBy(a => a.CaptionOrder).Select(a => a.Caption).Distinct();
				foreach (var caption in captions)
				{
					sb.AppendFormat("###{0}", caption);
					sb.AppendLine();
					foreach (var attribute in chapterAttributes.Where(a => a.Caption == caption).OrderBy(a => a.Order))
					{
						sb.AppendLine(attribute.Content);
						sb.AppendLine();
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine("___");
			}
			sb.AppendLine(Sidenotes);
			using (var writer = new StreamWriter("../../../README.md", false))
				writer.Write(sb.ToString());
		}

		private const string Introduction =
@"#QuickMGenerate

##Introduction
An evolution from the QuickGenerate library.

Aiming for : 
 - a terser (Linq) syntax 
 - a better way of dealing with state
 - better composability of generators
 - better documentation
 - fun

---
";
		private const string Sidenotes =
@"##On a side note

QuickGenerate has a lot of mostly unused and undocumented features.

These will be left out, but an easy means of implementing them yourselves, when needed, will be provided.

In contrary to my usual disdain for Extension Methods, QuickMGenerate makes heavy use of them.

Par example, ... casting generators :

```
public static Generator<State, string> AsString<T>(this Generator<State, T> generator)
{
	return s => new Result<State, string>(generator(s).Value.ToString(), s);
}
```

Once you figure out the Generator Delegate, I reckon a lot of extensability is available to you through custom extension methods and it doesn't flood your intellisense because of the specific types.

F.i. the Nullable extension only shows up on generators for structs.

In future the TState generic type of the Generator will be introduced in the MGen class methods.

This will allow for an extension of the State object that is threaded around through the generators.

Something that 'll be really usefull for QuickDotNetCheck for one.
";
	}
}