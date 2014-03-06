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

 - Passing in a non Enum type for T throws an ArgumentException.


###Custom Primitive Generators
Any function that returns a value of type `Generator<State, T>` can be used as an MGen generator.

Generator is defined as a delegate like so :
```
public delegate IResult<TState, TValue> Generator<TState, out TValue>(TState input)
```


So f.i. to define a generator that always returns the number forty-two we need a function that returns the following :
```
return s => new Result<State, int>(s.Random.Next(42, 42), s);
```

As you can see from the signature a state object is passed to the generator.
This is where the random seed lives.
If you want any kind of random, it is advised to use that one, like so :
```
return s => new Result<State, int>(s.Random.Next(42, 42), s);
```



___
##Generating Objects
###A simple object.
Use `MGen.One<T>()`, where T is the type of object you want to generate.

- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.

- The enumeration properties of the object will be automatically filled in using the default (or replaced) MGen.Enum<T> generator.



___
##On a side note

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

