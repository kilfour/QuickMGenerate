#QuickMGenerate

##Introduction
An evolution from the QuickGenerate library.

Aiming for : 
 - A terser (Linq) syntax.
 - A better way of dealing with state.
 - Better composability of generators.
 - Better documentation.
 - Fun.


 ---

##Generating Primitives
###Introduction
The MGen class has many methods which can be used to obtain a corresponding primitive.

F.i. `MGen.Int()`. 

Full details below in the chapter 'The Primitive Generators'.



___
##Combining Generators
###Linq Syntax.
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



___
##Generating Objects
###A simple object.
Use `MGen.One<T>()`, where T is the type of object you want to generate.

- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.

- The enumeration properties of the object will be automatically filled in using the default (or replaced) MGen.Enum<T> generator.

- Also works for properties with private setters.

- Can generate any object that has a parameterless constructor, be it public, protected, or private.

- The overload `MGen.One<T>(Func<T> constructor)` allows for specific constructor selection.


###Ignoring properties.
Use the `MGen.For<T>().Ignore<TProperty>(Expression<Func<T, TProperty>> func)` method chain.

F.i. :
```
MGen.For<SomeThingToGenerate>().Ignore(s => s.Id)
```

The property specified will be ignored during generation.

Derived classes generated also ignore the base property.

*Note :* The Ignore 'generator' does not actually generate anything, it only influences further generation.


###Customizing properties.
Use the `MGen.For<T>().Customize<TProperty>(Expression<Func<T, TProperty>> func, Generator<State, T>)` method chain.

F.i. :
```
MGen.For<SomeThingToGenerate>().Customize(s => s.MyProperty, MGen.Constant(42))
```

The property specified will be generated using the passed in generator.

An overload exists which allows for passing a value instead of a generator.

*Note :* The Customize 'generator' does not actually generate anything, it only influences further generation.


###Many objects.
Use The `.Many(int number)` generator extension.

The generator will generate an IEnumerable<T> of `int number` elements where T is the result type of the extended generator.


###ToArray.
Use The `.ToArray()` generator extension.

The `Many` generator above returns an IEnumerable.
This means it's value would be regenerated if we were to iterate over it more than once.
Use `ToArray` to *fix* the IEnumerable in place, so that it will return the same result with each iteration.
It can also be used to force evaluation in case the IEnumerable is not enumerated over because there's nothing in your select clause
referencing it. 



###Replacing Primitive Generators
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

*Note :* The Replace 'generator' does not actually generate anything, it only influences further generation.



___
##Generating Hierarchies
###Relations.
In the same way one can `Customize` primitives, this can also be done for references.

E.g. :

```
var generator =
	from product in MGen.One<ProductItem>()
	from setProduct in MGen.For<OrderLine>().Customize(order => order.Product, product)
	from orderline in MGen.One<OrderLine>()
	select orderline;
```


In case of a one-to-many relation where the collection is inaccessible, but a method is provided for adding the many to the one,
we can use the `Apply` method, which is explained in detail in the chapter 'Other Usefull Generators'.
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



###A 'Component'.
Use the `MGen.For<T>().Component()`, method chain.

Once a component is defined, from then on it is automatically generated for any object that has a property of the components type,
similarly to how primitives are handled.

The only exception to the component rule is when it would lead to an infinite loop.

*Note :* The Component 'generator' does not actually generate anything, it only influences further generation.



___
##Other Usefull Generators
###Apply.
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
This is usefull when dealing with objects and you just don't want to return said object.
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



###Picking an element out of a range.
Use `MGen.ChooseFrom<T>(IEnumerable<T> values)`.

Picks a random value from a list of options.

F.i. `MGen.ChooseFrom(new []{ 1, 2 })` will return either 1 or 2.

A helper method exists for ease of use when you want to pass in constant values as in the example above. 

I.e. : `MGen.ChooseFromThese(1, 2)`


###Generating unique values.
Use the `.Unique(object key)` extension method.

Makes sure that every generated value is unique.

When asking for more unique values than the generator can supply, an exception is thrown.

Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.

When using the same key for multiple unique generators all values across these generators are unique.


###Casting Generators.
Various extension methods allow for casting the generated value.

 - `.AsString()` : Invokes `.ToString()` on the generated value and 
casts the generator from `Generator<T>` to `Generator<string>`. 
Usefull f.i. to generate numeric strings.

 - `.AsObject()` : Simply casts the generator itself from `Generator<T>` to `Generator<object>`. Mostly used internally.


###'Generating' constants.
Use `MGen.Constant<T>(T value)`.

This generator is most usefull in combination with others and is used to inject constants into combined generators.



___
##The Primitive Generators
###Integers.
Use `MGen.Int()`.

The overload `MGen.Int(int min, int max)` generates an int higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `int?` using the `.Nullable()` extension.

 - `int` is automatically detected and generated for object properties.

 - `Int32` is automatically detected and generated for object properties.

 - `int?` is automatically detected and generated for object properties.


###Chars.
Use `MGen.Char()`. 

No overload Exists.

The default generator always generates a char between lower case 'a' and lower case 'z'.

Can be made to return `char?` using the `.Nullable()` extension.

 - `char` is automatically detected and generated for object properties.

 - `char?` is automatically detected and generated for object properties.


###Strings.
Use `MGen.String()`.

The generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.

The overload `MGen.String(int min, int max)` generates an string of length higher or equal than min and lower than max.

The Default generator generates a string of length higher than 0 and lower than 10.

 - `string` is automatically detected and generated for object properties.


###Booleans.
Use `MGen.Bool()`. 

No overload Exists.

The default generator generates True or False.

Can be made to return `bool?` using the `.Nullable()` extension.

 - `bool` is automatically detected and generated for object properties.

 - `bool?` is automatically detected and generated for object properties.


###Decimals.
Use `MGen.Decimal()`.

The overload `MGen.Decimal(decimal min, decimal max)` generates a decimal higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `decimal?` using the `.Nullable()` extension.

 - `decimal` is automatically detected and generated for object properties.

 - `decimal?` is automatically detected and generated for object properties.


###DateTimes.
Use `MGen.DateTime()`.

The overload `MGen.DateTimes(DateTime min, DateTime max)` generates a DateTime higher or equal than min and lower than max.

The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).

Can be made to return `DateTime?` using the `.Nullable()` extension.

 - `DateTime` is automatically detected and generated for object properties.

 - `DateTime?` is automatically detected and generated for object properties.


###Longs.
Use `MGen.Long()`.

The overload `MGen.Long(long min, long max)` generates a long higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `long?` using the `.Nullable()` extension.

 - `long` is automatically detected and generated for object properties.

 - `Int64` is automatically detected and generated for object properties.

 - `long?` is automatically detected and generated for object properties.


###Doubles.
Use `MGen.Double()`.

The overload `MGen.Double(double min, double max)` generates a double higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `double?` using the `.Nullable()` extension.

 - `double` is automatically detected and generated for object properties.

 - `double?` is automatically detected and generated for object properties.


###Floats.
Use `MGen.Float()`.

The overload `MGen.Float(float min, float max)` generates a float higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `float?` using the `.Nullable()` extension.

 - `float` is automatically detected and generated for object properties.

 - `float?` is automatically detected and generated for object properties.


###Guids.
Use `MGen.Guid()`.

There is no overload.

The default generator never generates Guid.Empty.

Can be made to return `Guid?` using the `.Nullable()` extension.

 - `Guid` is automatically detected and generated for object properties.

 - `Guid?` is automatically detected and generated for object properties.


###Shorts.
Use `MGen.Short()`.

The overload `MGen.Short(short min, short max)` generates a short higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `short?` using the `.Nullable()` extension.

 - `short` is automatically detected and generated for object properties.

 - `short?` is automatically detected and generated for object properties.


###TimeSpans.
Use `MGen.TimeSpan()`.

The overload `MGen.TimeSpan(int max)` generates a TimeSpan with Ticks higher or equal than 1 and lower than max.

The default generator is (max = 1000).

Can be made to return `TimeSpan?` using the `.Nullable()` extension.

 - `TimeSpan` is automatically detected and generated for object properties.

 - `TimeSpan?` is automatically detected and generated for object properties.


###Enumerations.
Use `MGen.Enum<T>()`, where T is the type of Enum you want to generate. 

No overload Exists.

The default generator just picks a random value from all enemeration values.

 - An Enumeration is automatically detected and generated for object properties.

 - A nullable Enumeration is automatically detected and generated for object properties.

 - Passing in a non Enum type for T throws an ArgumentException.



___
##Creating Custom Generators
###How To
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

See also : [Creating a counter example](./QuickMGenerate.Tests/CreatingCustomGenerators/CreatingACounterGeneratorExample.cs).



___
##After Thoughts

Well ... 
Goals achieved I reckon.
 * **A terser (Linq) syntax** :
For some who are not used it, it might get tricky to get into. 
I must say I myself only started using it when I started using [Sprache](https://github.com/sprache/Sprache). 
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


