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
			}

			using (var writer = new StreamWriter("../../../README.md", false))
				writer.Write(sb.ToString());
			Console.WriteLine(sb.ToString());
		}

		private const string Introduction = 
@"#QuickMGenerate

##Introduction
An evolution from the QuickGenerate library.

Aiming for : 
 - a terser (Linq) syntax 
 - a better way of dealing with state
 - better composability of generators
 - fun

";
	}
}