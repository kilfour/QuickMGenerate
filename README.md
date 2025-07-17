# QuickMGenerate
> A type-walking cheetah with a hand full of random.
## Introduction
An evolution from the QuickGenerate library.

Aiming for : 
- A terser (Linq) syntax.
- A better way of dealing with state.
- Better composability of generators.
- Better documentation.
- Fun.


 ---

## Generating Primitives
### Introduction
The MGen class has many methods which can be used to obtain a corresponding primitive.

F.i. `MGen.Int()`. 

Full details below in the chapter 'The Primitive Generators'.



___
## Combining Generators
### Linq Syntax.
Each MGen Generator can be used as a building block and combined using query expressions.

F.i. the following :
```
var stringGenerator =
	from a in MGen.Int()
	from b in MGen.String()
	from c in MGen.Int()
	select a + b + c;
Console.WriteLine(stringGenerator.Generate());
```
Will output something like `28ziicuiq56`.

Generators are reusable building blocks. 

In the following :
```
var generator =
	from str in stringGenerator.Replace()
	from thing in MGen.One<SomeThingToGenerate>()
	select thing;
```
We reuse the 'stringGenerator' defined above and replace the default string generator with our custom one. 
All strings in the generated object will have the pattern defined by 'stringGenerator'.

This approach removes the problem of combinatoral explosion. No need for a Transform<T, U>(...) combinator for example
as this can be easily achieved using Linq. 

```
var generator =
	from chars in MGen.Constant('-').Many(5)
	let composed = chars.Aggregate(", (a, b) => a + b.ToString())
	select composed;
```
Generates: "-----".


### Using Extensions.
When applying the various extension methods onto a generator, they get *combined* into a new generator.

Jumping slightly ahead of ourselves as below example will use methods that are explained more thoroughly further below.

The old quickgenerate had a *PickOne()* method, which randomly picked an element from an IEnumerable.

This has now been replaced with `MGen.ChooseFrom()` and `MGen.ChooseFromThese()` (see Chapter 'Other Useful Generators').

QuickGenerate also had a *PickMany(int number)* method which picked *number* amount of elements from an IEnumerable 
and also made sure that it picked different elements.

The PickMany method is now obsolete as the same thing can be achieved through generator composition.

E.g. :
```
MGen.ChooseFrom(someValues).Unique("key").Many(2)
```

In the same vein, I was able to leave a lot of code out, and at the same time, probably providing more features.




___
## Generating Objects
### A simple object.
Use `MGen.One<T>()`, where T is the type of object you want to generate.

- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.

- The enumeration properties of the object will be automatically filled in using the default (or replaced) MGen.Enum<T> generator.

- The object properties will also be automatically filled in using the default (or replaced) generators, similar to calling MGen.One<TProperty>() and setting the value using `Apply` (see below) explicitely.

- Also works for properties with private setters.

- Can generate any object that has a parameterless constructor, be it public, protected, or private.

- The overload `MGen.One<T>(Func<T> constructor)` allows for specific constructor selection.


### Ignoring properties.
Use the `MGen.For<T>().Ignore<TProperty>(Expression<Func<T, TProperty>> func)` method chain.

F.i. :
```
MGen.For<SomeThingToGenerate>().Ignore(s => s.Id)
```

The property specified will be ignored during generation.

Derived classes generated also ignore the base property.

Sometimes it is useful to ignore all properties while generating an object.  
For this use `MGen.For<SomeThingToGenerate>().IgnoreAll()`

`IgnoreAll()` does not ignore properties on derived classes, even inherited properties.

**Note :** `The Ignore(...)` combinator does not actually generate anything, it only influences further generation.


### Customizing properties.
Use the `MGen.For<T>().Customize<TProperty>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
MGen.For<SomeThingToGenerate>().Customize(s => s.MyProperty, MGen.Constant(42))
```

The property specified will be generated using the passed in generator.

An overload exists which allows for passing a value instead of a generator.

Derived classes generated also use the custom property.

*Note :* The Customize combinator does not actually generate anything, it only influences further generation.


### Customizing constructors.
Use the `MGen.For<T>().Construct<TArg>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
MGen.For<SomeThing>().Construct(MGen.Constant(42))
```

Subsequent calls to `MGen.One<T>()` will then use the registered constructor.

Various overloads exist : 
 -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2)`

 -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3)`

 -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4)`

 -  `MGen.For<T>().Construct<T1, T2>(Generator<T1> g1, Generator<T2> g2, Generator<T3> g3, Generator<T4> g4, Generator<T5> g5)`  

After that, ... you're on your own.

Or use the factory method overload:  
`MGen.For<T>().Construct<T>(Func<T> ctor)`

*Note :* The Construct combinator does not actually generate anything, it only influences further generation.


### Many objects.
Use The `.Many(int number)` generator extension.

The generator will generate an IEnumerable<T> of `int number` elements where T is the result type of the extended generator.

An overload exists (`.Many(int min, int max`) where the number of elements is in between the specified arguments.


### ToArray.
Use The `.ToArray()` generator extension.

The `Many` generator above returns an IEnumerable.
This means it's value would be regenerated if we were to iterate over it more than once.
Use `ToArray` to *fix* the IEnumerable in place, so that it will return the same result with each iteration.
It can also be used to force evaluation in case the IEnumerable is not enumerated over because there's nothing in your select clause
referencing it. 



### Inheritance.
Use The `MGen.For<T>().GenerateAsOneOf(params Type[] types)` method chain.

F.i. :
```
MGen.For<SomeThingAbstract>().GenerateAsOneOf(
	typeof(SomethingDerived), typeof(SomethingElseDerived))
```

When generating an object of type T, an object of a random chosen type from the provided list will be generated instead.

**Note :** The `GenerateAsOneOf(...)` combinator does not actually generate anything, it only influences further generation.


### ToList.
Use The `.ToList()` generator extension.

Similar to the `ToArray` method. But instead of an Array, this one returns a, you guessed it, List. 



### Replacing Primitive Generators
Use the `.Replace()` extension method.

Example
```
var generator =
	from _ in MGen.Constant(42).Replace()
	from result in MGen.One<SomeThingToGenerate>()
	select result;
```
When executing above generator it will return a SomeThingToGenerate object where all integers have the value 42.


Replacing a primitive generator automatically impacts it's nullable counterpart.

Replacing a nullable primitive generator does not impacts it's non-nullable counterpart.

Replacements can occur multiple times during one generation :
```
var generator =
	from _ in MGen.Constant(42).Replace()
	from result1 in MGen.One<SomeThingToGenerate>()
	from __ in MGen.Constant(666).Replace()
	from result2 in MGen.One<SomeThingToGenerate>()
	select new[] { result1, result2 };
```
When executing above generator result1 will have all integers set to 42 and result2 to 666.

*Note :* The Replace combinator does not actually generate anything, it only influences further generation.



___
## Generating Hierarchies
### Relations.
In the same way one can `Customize` primitives, this can also be done for references.

E.g. :

```
var generator =
	from product in MGen.One<ProductItem>()
	from setProduct in MGen.For<OrderLine>().Customize(orderline => orderline.Product, product)
	from orderline in MGen.One<OrderLine>()
	select orderline;
```


In case of a one-to-many relation where the collection is inaccessible, but a method is provided for adding the many to the one,
we can use the `Apply` method, which is explained in detail in the chapter 'Other Useful Generators'.
E.g. :

```
var generator =
	from order in MGen.One<Order>()
	from addLine in MGen.For<OrderLine>().Apply(order.AddOrderLine)
	from lines in MGen.One<OrderLine>().Many(20).ToArray()
	select order;
```
Note the `ToArray` call on the orderlines. 
This forces enumeration and is necessary because the lines are not enumerated over just by selecting the order.


If we were to select the lines instead of the order, `ToArray` would not be necessary.

Relations defined by constructor injection can be generated using the `One<T>(Func<T> constructor)` overload.
E.g. :

```
var generator =
	from category in MGen.One<Category>()
	from subCategory in MGen.One(() => new SubCategory(category)).Many(20)
	select category;
```



### Depth Control.
As mentioned in the *A simple object section*: “The object properties will also be automatically filled in.”
However, this automatic population only applies to the first level of object properties.
Deeper properties will remain null unless configured otherwise.  
So if we have the following class :
```csharp
public class NoRecurse { }
public class Recurse
{
	public Recurse Child { get; set; }
	public NoRecurse OtherChild { get; set; }
	public override string ToString()
	{
		var childString =
			Child == null ? "<null>" : Child.ToString();
		var otherChildString =
			OtherChild == null ? "<null>" : "{ NoRecurse }";
		return $"{{ Recurse: Child = {childString}, OtherChild = {otherChildString} }}";
	}
}
```

If we then do :
```csharp
Console.WriteLine(MGen.One<Recurse>().Generate().ToString());
```
It outputs : 
```
{ Recurse: Child = <null>, OtherChild = { NoRecurse } }
```
While this may seem counter-intuitive, it is an intentional default to prevent infinite recursion or overly deep object trees.
Internally, a `DepthConstraint(int Min, int Max)` is registered per type.
The default values are `new(1, 1)`.  
Revisiting our example we can see that both types have indeed been generated with these default values.

You can control generation depth per type using the `.Depth(min, max)` combinator.  
For instance:
```csharp
var generator =
	from _ in MGen.For<Recurse>().Depth(2, 2)
	from recurse in MGen.One<Recurse>()
	select recurse;
Console.WriteLine(generator.Generate().ToString());
```
Outputs:
```
{ Recurse: Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } }
, OtherChild = { NoRecurse } 
}
```
 

Recap:
```
Depth(1, 1)
{ Recurse: Child = <null>, OtherChild = { NoRecurse } }

Depth(2, 2)
{ Recurse: 
	Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } },
  	OtherChild = { NoRecurse } 
}

Depth(3, 3)
{ Recurse: 
	Child = { Recurse: 
		Child = { Recurse: Child = <null>, OtherChild = { NoRecurse } },
        OtherChild = { NoRecurse } },
  	OtherChild = { NoRecurse } 
}
```
 

Using for instance `.Depth(1, 3)` allows the generator to randomly choose a depth between 1 and 3 (inclusive) for that type.
This means some instances will be shallow, while others may be more deeply nested, introducing variability within the defined bounds.

**Note :** The `Depth(...)` combinator does not actually generate anything, it only influences further generation.


### Trees.
Depth control together with the `.GenerateAsOneOf(...)` combinator mentioned above and the previously unmentioned `TreeLeaf<T>()` one allows you to build tree type hierarchies.  
Given the cannonical abstract Tree, concrete Branch and Leaf example model, we can generate this like so:
```csharp
var generator =
	from _d in MGen.For<Tree>().Depth(1, 3)
	from _i in MGen.For<Tree>().GenerateAsOneOf(typeof(Branch), typeof(Leaf))
	from _l in MGen.For<Tree>().TreeLeaf<Leaf>()
	from tree in MGen.One<Tree>()
	select tree;
```
Our leaf has an int value property, so the following:
```csharp
Console.WriteLine(generator.Generate().ToString());
```	
Would output something like:
```
Node(Leaf(31), Node(Leaf(71), Leaf(10)))
```


**Note :** The `TreeLeaf<T>()` combinator does not actually generate anything, it only influences further generation.



___
## Other Useful Generators
### Apply.
Use the `.Apply<T>(Func<T, T> func)` extension method.

Applies the specified Function to the generated value, returning the result.
F.i. `MGen.Constant(41).Apply(i =>  i + 1)` will return 42.

Par example, when you want all decimals to be rounded to a certain precision : 
```
var generator = 
	from _ in MGen.Decimal().Apply(d => Math.Round(d, 2)).Replace()
	from result in MGen.One<SomeThingToGenerate>()
	select result;
```

An overload exists with signature `Apply<T>(Action<T> action)`.
This is useful when dealing with objects and you just don't want to return said object.
E.g. `MGen.One<SomeThingToGenerate>().Apply(session.Save)`.

This function also exists as a convention instead of a generator.

E.g. `MGen.For<SomeThingToGenerate>().Apply(session.Save)`.

In this case nothing is generated but instead the function will be applied to all objects of type T during generation.

There is no `MGen.For<T>().Apply(Func<T, T> func)` as For can only be used for objects, so there is no need for it really.


Lastly the convention based `Apply` has an overload which takes another generator.
This generator then provides a value which can be used in the action parameter.

E.g. : 
```
var parents = ...
MGen.For<SomeChild>().Apply(MGen.ChooseFrom(parents), (child, parent) => parent.Add(child))
```



### Picking an element out of a range.
Use `MGen.ChooseFrom<T>(IEnumerable<T> values)`.

Picks a random value from a list of options.

F.i. `MGen.ChooseFrom(new []{ 1, 2 })` will return either 1 or 2.

A helper method exists for ease of use when you want to pass in constant values as in the example above. 

I.e. : `MGen.ChooseFromThese(1, 2)`

Another method provides a _semi-safe_ way to pick from what might be an empty list. 

I.e. : `MGen.ChooseFromWithDefaultWhenEmpty(new List<int>())`, which returns the default, in this case zero.

You can also pick from a set of Generators. 

I.e. : `MGen.ChooseGenerator(MGen.Constant(1), MGen.Constant(2))`


### Generating unique values.
Use the `.Unique(object key)` extension method.

Makes sure that every generated value is unique.

When asking for more unique values than the generator can supply, an exception is thrown.

Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.

When using the same key for multiple unique generators all values across these generators are unique.


### Filtering generated values.
Use the `.Where(Func<T, bool>)` extension method.

Makes sure that every generated value passes the supplied predicate.


### Casting Generators.
Various extension methods allow for casting the generated value.

 - `.AsString()` : Invokes `.ToString()` on the generated value and 
casts the generator from `Generator<T>` to `Generator<string>`. 
Useful f.i. to generate numeric strings.

 - `.AsObject()` : Simply casts the generator itself from `Generator<T>` to `Generator<object>`. Mostly used internally.

 - `.Nullable()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.

 - `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .


### How About Null(s)?
Various extension methods allow for influencing null generation.

- `.Nullable()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.  
> Used for value types.

- `.Nullable(int timesBeforeResultIsNullAproximation)` : overload of `Nullable()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .

- `.NullableRef()` : Casts a `Generator<T>` to `Generator<T?>`. In addition generates null 1 out of 5 times.  
> Used for reference types, including `string`.

- `.NullableRef(int timesBeforeResultIsNullAproximation)` : overload of `NullableRef()`, generates null 1 out of `timesBeforeResultIsNullAproximation` times .


### 'Generating' constants.
Use `MGen.Constant<T>(T value)`.

This generator is most useful in combination with others and is used to inject constants into combined generators.


### 'Never return null.
Use the `.NeverReturnNull()` extension method.`.

Only available on generators that provide `Nullable<T>` values, this one makes sure that, you guessed it, the nullable generator never returns null.



___
## The Primitive Generators
### Integers.
Use `MGen.Int()`.

The overload `MGen.Int(int min, int max)` generates an int higher or equal than min and lower than max.

Throws an ArgumentException if min > max.

The default generator is (min = 1, max = 100).

Can be made to return `int?` using the `.Nullable()` combinator.

 - `int` is automatically detected and generated for object properties.

 - `Int32` is automatically detected and generated for object properties.

 - `int?` is automatically detected and generated for object properties.


### Chars.
Use `MGen.Char()`. 

No overload Exists.

The default generator always generates a char between lower case 'a' and lower case 'z'.

Can be made to return `char?` using the `.Nullable()` combinator.

 - `char` is automatically detected and generated for object properties.

 - `char?` is automatically detected and generated for object properties.


### Strings.
Use `MGen.String()`.

The generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.

The overload `MGen.String(int min, int max)` generates an string of length higher or equal than min and lower than max.

The Default generator generates a string of length higher than 0 and lower than 10.

 - `string` is automatically detected and generated for object properties.

Can be made to return `string?` using the `.NullableRef()` combinator.


### Booleans.
Use `MGen.Bool()`. 

No overload Exists.

The default generator generates True or False.

Can be made to return `bool?` using the `.Nullable()` combinator.

 - `bool` is automatically detected and generated for object properties.

 - `bool?` is automatically detected and generated for object properties.


### Decimals.
Use `MGen.Decimal()`.

The overload `MGen.Decimal(decimal min, decimal max)` generates a decimal higher or equal than min and lower than max.

Throws an ArgumentException if min > max.

The default generator is (min = 1, max = 100).

Can be made to return `decimal?` using the `.Nullable()` combinator.

 - `decimal` is automatically detected and generated for object properties.

 - `decimal?` is automatically detected and generated for object properties.


### DateTimes.
Use `MGen.DateTime()`.

The overload `MGen.DateTimes(DateTime min, DateTime max)` generates a DateTime higher or equal than min and lower than max.

The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).

Can be made to return `DateTime?` using the `.Nullable()` combinator.

 - `DateTime` is automatically detected and generated for object properties.

 - `DateTime?` is automatically detected and generated for object properties.


### Longs.
Use `MGen.Long()`.

The overload `MGen.Long(long min, long max)` generates a long higher or equal than min and lower than max.

Throws an ArgumentException if min > max.

The default generator is (min = 1, max = 100).

Can be made to return `long?` using the `.Nullable()` combinator.

 - `long` is automatically detected and generated for object properties.

 - `Int64` is automatically detected and generated for object properties.

 - `long?` is automatically detected and generated for object properties.


### Doubles.
Use `MGen.Double()`.

The overload `MGen.Double(double min, double max)` generates a double higher or equal than min and lower than max.

Throws an ArgumentException if min > max.

The default generator is (min = 1, max = 100).

Can be made to return `double?` using the `.Nullable()` combinator.

 - `double` is automatically detected and generated for object properties.

 - `double?` is automatically detected and generated for object properties.


### Floats.
Use `MGen.Float()`.

The overload `MGen.Float(float min, float max)` generates a float higher or equal than min and lower than max.

Throws an ArgumentException if min > max.

The default generator is (min = 1, max = 100).

Can be made to return `float?` using the `.Nullable()` combinator.

 - `float` is automatically detected and generated for object properties.

 - `float?` is automatically detected and generated for object properties.


### Guids.
Use `MGen.Guid()`.

There is no overload.

The default generator never generates Guid.Empty.

Can be made to return `Guid?` using the `.Nullable()` combinator.

 - `Guid` is automatically detected and generated for object properties.

 - `Guid?` is automatically detected and generated for object properties.


### Shorts.
Use `MGen.Short()`.

The overload `MGen.Short(short min, short max)` generates a short higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `short?` using the `.Nullable()` combinator.

 - `short` is automatically detected and generated for object properties.

 - `short?` is automatically detected and generated for object properties.


### TimeSpans.
Use `MGen.TimeSpan()`.

The overload `MGen.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.

The default generator is (max = 1000).

Can be made to return `TimeSpan?` using the `.Nullable()` combinator.

 - `TimeSpan` is automatically detected and generated for object properties.

 - `TimeSpan?` is automatically detected and generated for object properties.


### Enums.
Use `MGen.Enum<T>()`, where T is the type of Enum you want to generate. 

No overload Exists.

The default generator just picks a random value from all enemeration values.

 - An Enumeration is automatically detected and generated for object properties.

 - A nullable Enumeration is automatically detected and generated for object properties.

 - Passing in a non Enum type for T throws an ArgumentException.



___
## Creating Custom Generators
### How To
Any function that returns a value of type `Generator<T>` can be used as a generator.

Generator is defined as a delegate like so :
```
public delegate IResult<TValue> Generator<out TValue>(State input)
```


So f.i. to define a generator that always returns the number forty-two we need a function that returns the following :
```
return s => new Result<State, int>(42, s);
```

As you can see from the signature a state object is passed to the generator.
This is where the random seed lives.
If you want any kind of random, it is advised to use that one, like so :
```
return s => new Result<State, int>(s.Random.Next(42, 42), s);
```

See also : [Creating a counter generator](./QuickMGenerate.Tests/CreatingCustomGenerators/CreatingACounterGeneratorExample.cs).



___
## On Scope
### Overview
Linq chains especially in query syntax can be confusing when it comes to scope.
But once you understand the basic rule, everything will quickly seem obvious.
For generating trivial and even less than trivial examples, you can likely ignore this chapter completely.
However for complex stuff where generators get reused, sometimes in implicit ways, 
it is good to know what *exactly* is going on.


**Note:** This section is still being worked on, more information will follow.



___
## After Thoughts

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


