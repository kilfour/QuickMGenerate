using System;
using System.Text;
using QuickMGenerate.Data;
using Xunit;

namespace QuickMGenerate.Tests
{
	public class GeneratingNamesSpike
	{
		[Fact]
		public void Spiky()
		{
			var generator =
				from firstname in MGen.ChooseFromThese(DataLists.FirstNames)
				from lastname in MGen.ChooseFromThese(DataLists.LastNames)
				from provider in MGen.ChooseFromThese("yahoo", "gmail", "mycompany")
				from domain in MGen.ChooseFromThese("com", "net", "biz")
				let email = string.Format("{0}.{1}@{2}.{3}", firstname, lastname, provider, domain)
				select
					new Person
						{
							FirstName = firstname,
							LastName = lastname,
							Email = email
						};
			var people = generator.Many(20).Generate();
			foreach (var person in people)
			{
				Console.Write(person);
			}
		}

		public class Person
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string Email { get; set; }

			public override string ToString()
			{
				var str = new StringBuilder();
				str.AppendFormat("Name : {0} {1}, Email : {2}.", FirstName, LastName, Email);
				str.AppendLine();
				return str.ToString();
			}
		}
	}
}