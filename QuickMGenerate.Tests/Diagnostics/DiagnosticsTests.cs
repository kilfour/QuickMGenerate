﻿// using QuickMGenerate.Diagnostics;
// using QuickMGenerate.Diagnostics.Inspectors;


// namespace QuickMGenerate.Tests.Diagnostics;

// [DiagnosticsInspecting(
// 	Content =
// @"QuickMGenerate allows you to inspect generated values through use of the `Inspect<T>(...)` combinator:
// ```
// Inspect<T>(this Generator<T> generator, Func<T, (string[] tags, string message, object data)> describe)
// ```
// Furthermore, three overloads are provided to remove unnecessary ceremony :
// ```
// Inspect<T>(this Generator<T> generator, string[] tags, string message)
// Inspect<T>(this Generator<T> generator, string[] tags)
// Inspect<T>(this Generator<T> generator)
// ```
// In order to retrieve inspected values a concrete instance of `Inspector` must be registered.  
// Example :
// ```
// InspectorContext.Current = myConcreteInspectorInstance;
// MGen.Constant(42).Inspect(a => ([""my tag""], ""a log message"", new { label = ""constant"", value = a }));
// ```
// Several `Inspector` types are provided by QuickMGenerate.
// ",
// 	Order = 0)]
// public class DiagnosticsTests
// {
// 	[DiagnosticsInspecting(
// 	Content =
// @"**Note:** This section is still being worked on, more information will follow.",
// 	Order = 0)]
// 	[Fact]
// 	public void UsageExample()
// 	{
// 		var valueLogger = new ValueLogger<object>();
// 		InspectorContext.Current = valueLogger;
// 		var generator = MGen.Constant(42).Inspect();
// 		generator.Generate();
// 		Assert.Single(valueLogger.Values);
// 		Assert.Equal(42, valueLogger.Values[0]);
// 	}

// 	[DiagnosticsInspecting(
// 	Content =
// @"MULTIPLE INSPECTORS 

// **Note:** This section is still being worked on, more information will follow.",
// 	Order = 0)]
// 	[Fact]
// 	public void Multiples()
// 	{
// 		var writer = new WriteToJsonFile();
// 		var valueLogger = new ValueLogger<object>();
// 		InspectorContext.Current = valueLogger;
// 		var generator = MGen.Constant(42).Inspect();
// 		generator.Generate();
// 		Assert.Single(valueLogger.Values);
// 		Assert.Equal(42, valueLogger.Values[0]);
// 	}

// 	public class DiagnosticsInspectingAttribute : DiagnosticsAttribute
// 	{
// 		public DiagnosticsInspectingAttribute()
// 		{
// 			Caption = "Inspecting";
// 			CaptionOrder = 0;
// 		}
// 	}
// }