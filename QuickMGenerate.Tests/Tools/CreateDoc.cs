using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace QuickMGenerate.Tests.Tools
{
	public class CreateDoc
	{
		[Fact]
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
			sb.AppendLine(AfterThoughts);
			using (var writer = new StreamWriter("../../../README.md", false))
				writer.Write(sb.ToString());
		}

		private const string Introduction =
@"#QuickMGenerate

##Introduction
An evolution from the QuickGenerate library.

Aiming for : 
 - A terser (Linq) syntax.
 - A better way of dealing with state.
 - Better composability of generators.
 - Better documentation.
 - Fun.


 ---
";
		private const string AfterThoughts =
@"##After Thoughts

Well ... 
Goals achieved I reckon.
 * **A terser (Linq) syntax** :
For some who are not used it, it might get tricky to get into. 
I must say, I myself, only started using it when I started using [Sprache](https://github.com/sprache/Sprache). 
A beautifull Parsec inspired parsing library.
Stole some ideas from there, I must admit.

 * **A better way of dealing with state, better composability of generators** :
Here's an example of something simple that was quite hard to do in the old QuickGenerate :

```
var generator =
	from firstname in MGen.ChooseFromThese(DataLists.FirstNames)
	from lastname in MGen.ChooseFromThese(DataLists.LastNames)
	from provider in MGen.ChooseFromThese(""yahoo"", ""gmail"", ""mycompany"")
	from domain in MGen.ChooseFromThese(""com"", ""net"", ""biz"")
	let email = string.Format(""{0}.{1}@{2}.{3}"", firstname, lastname, provider, domain)
	select
		new Person
			{
				FirstName = firstname,
				LastName = lastname,
				Email = email
			};
var people = generator.Many(2).Generate();
foreach (var person in people)
{
	Console.Write(person);
}
```
Which outputs something like :
```
  Name : Claudia Coffey, Email : Claudia.Coffey@gmail.net.
  Name : Dale Weber, Email : Dale.Weber@mycompany.biz.
```
 * **Better documentation** : You're looking at it.
 * **Fun** : Well, yes it was.

Even though QuickMGenerate uses a lot of patterns (there's static all over the place) that I usually frown upon,
It's a lot less code, it's a lot more composable, it's, ... well, ... what QuickGenerate should have been.

";
	}
}