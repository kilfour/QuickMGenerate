#QuickMGenerate

##Introduction
An evolution from the QuickGenerate library.

Aiming for : 
 - a terser (Linq) syntax 
 - a better way of dealing with state
 - better composability of generators
 - better documentation
 - fun

**Warning** :  This Lib is still in Alpha ;-).
Interface subject to change.

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
###A 'Component'.
Use the `MGen.For<T>().Component()`, method chain.

Once a component is defined, from then on it is automatically generated for any object that has a property of the components type,
similarly to how primitives are handled.

*Note :* The Component 'generator' does not actually generate anything, it only influences further generation.



___
##Other Usefull Generators
###'Generating' constants.
Use `MGen.Constant<T>(T value)`.

This generator is most usefull in combination with others and is used to inject constants into combined generators.


###Picking an element out of a range.
Use `MGen.ChooseFrom<T>(IEnumerable<T> values)`.

Picks a random value from a list of options.

F.i. `MGen.ChooseFrom(new []{ 1, 2 })` will return either 1 or 2.

A helper method exists for ease of use when you want to pass in constant values as in the example above. I.e. : `MGen.ChooseFromThese(1, 2)`


###Generating unique values.
Use the `.Unique(object key)` extension method.

Makes sure that every generated value is unique.

When asking for more unique values than the generator can supply, an exception is thrown.

Multiple unique generators can be defined in one 'composed' generator, without interfering with eachother by using a different key.

When using the same key for multiple unique generators all values across these generators are unique.


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



###Casting Generators.
Various extension methods allow for casting the generated value.

 - `.AsString()` : Invokes `.ToString()` on the generated value and 
casts the generator from `Generator<State, T>` to `Generator<State, string>`. 
Usefull f.i. to generate numeric strings.

 - `.AsObject()` : Simply casts the generator itself from `Generator<State, T>` to `Generator<State, object>`. Mostly used internally.



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
Any function that returns a value of type `Generator<State, T>` can be used as an MGen generator.

Generator is defined as a delegate like so :
```
public delegate IResult<TState, TValue> Generator<TState, out TValue>(TState input)
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



___
##On a side note

The old QuickGenerate has a lot of mostly unused and undocumented features.

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

