#QuickMGenerate

##Introduction
An evolution from the QuickGenerate library.

Aiming for : 
 - a terser (Linq) syntax 
 - a better way of dealing with state
 - better composability of generators
 - fun

Also QuickGenerate has a lot of mostly unused and undocumented features.

These will be left out, but an easy means of implementing them yourselves, when needed, will be provided.

In contrary to my usual disdain for Extension Methods, QuickMGenerate makes heavy use of them.

Par example :

Casting generators :

```
public static Generator<State, string> AsString<T>(this Generator<State, T> generator)
{
	return s => new Result<State, string>(generator(s).Value.ToString(), s);
}```

Once you figure out the Generator Delegate, I reckon a lot of extensability is available to you through this method and it doesn't flood your intellisense because of the specific types.

F.i. the Nullable extension only shows up on generators for structs.

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



___
