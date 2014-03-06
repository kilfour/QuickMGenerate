#QuickMGenerate

##Introduction
An evolution from the QuickGenerate library.

Aiming for : 
 - a terser (Linq) syntax 
 - a better way of dealing with state
 - better composability of generators
 - better documentation
 - fun

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

Keep in mind that the .Replace() call returns a 'Unit' generator. 
I.e. it does not really generate anything on it's own.


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



___
##Other usefull Generators
###'Generating' constants.
Use `MGen.Constant<T>(T value)`.

This generator is most usefull in combination with others and is used to inject constants into combined generators.


###Picking an element out of a range.
Use `MGen.ChooseFrom<T>(params T[] values)`.

Picks a random value from a list of options.

F.i. `MGen.ChooseFrom(1, 2)` will return either 1 or 2.


###Generating unique values.
Use the `.Unique()` extension method.

Makes sure that every generated value is unique.

When asking for more unique values than the generator can supply, an exception is thrown.


###Casting Generators.
Various extension methods allow for casting the generated value.

 - `.AsString()` : Invokes `.ToString()` on the generated value and 
casts the generator from `Generator<State, T>` to `Generator<State, object>`. 
Usefull f.i. to generate numeric strings.

 - `.AsObject()` : Simply casts the generator itself from `Generator<State, T>` to `Generator<State, object>`. Mostly used internally.



___
##The Primitive Generators
###Integers
Use `MGen.Int()`.

The overload `MGen.Int(int min, int max)` generates an int higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `int?` using the `.Nullable()` extension.

 - `int` is automatically detected and generated for object properties.

 - `Int32` is automatically detected and generated for object properties.

 - `int?` is automatically detected and generated for object properties.


###Chars
Use `MGen.Char()`. 

No overload Exists.

The default generator always generates a char between lower case 'a' and lower case 'z'.

Can be made to return `char?` using the `.Nullable()` extension.

 - `char` is automatically detected and generated for object properties.

 - `char?` is automatically detected and generated for object properties.


###Strings
Use `MGen.String()`. 

No overload Exists.

The default generator always generates every char element of the string to be between lower case 'a' and lower case 'z'.

The Default generator generates a string of length higher than 0 and lower than 10.

 - `string` is automatically detected and generated for object properties.


###Booleans
Use `MGen.Bool()`. 

No overload Exists.

The default generator generates True or False.

Can be made to return `bool?` using the `.Nullable()` extension.

 - `bool` is automatically detected and generated for object properties.

 - `bool?` is automatically detected and generated for object properties.


###Decimals
Use `MGen.Decimal()`.

The overload `MGen.Decimal(int min, int max)` generates an int higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `decimal?` using the `.Nullable()` extension.

 - `decimal` is automatically detected and generated for object properties.

 - `decimal?` is automatically detected and generated for object properties.


###DateTimes
Use `MGen.DateTime()`.

The overload `MGen.DateTimes(DateTime min, DateTime max)` generates a DateTime higher or equal than min and lower than max.

The default generator is (min = new DateTime(1970, 1, 1), max = new DateTime(2020, 12, 31)).

Can be made to return `DateTime?` using the `.Nullable()` extension.

 - `DateTime` is automatically detected and generated for object properties.

 - `DateTime?` is automatically detected and generated for object properties.


###Longs
Use `MGen.Long()`.

The overload `MGen.Long(long min, long max)` generates a long higher or equal than min and lower than max.

The default generator is (min = 1, max = 100).

Can be made to return `long?` using the `.Nullable()` extension.

 - `long` is automatically detected and generated for object properties.

 - `Int64` is automatically detected and generated for object properties.

 - `long?` is automatically detected and generated for object properties.


###Enumerations
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

