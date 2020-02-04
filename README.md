# Just Another Infrastructure Library [![Build status](https://ci.appveyor.com/api/projects/status/thaf807e5s5jybwe?svg=true)](https://ci.appveyor.com/project/hoborg91/jail) [![Coverage Status](https://coveralls.io/repos/github/hoborg91/jail/badge.svg?branch=master)](https://coveralls.io/github/hoborg91/jail?branch=master)

This is a set of C# projects containing some infrastructure classes and extension methods I use frequently in my works.
I have decided to place it in a remote repository and make a NuGet package to be able to include it to every new project I need.
I do not suppose that someone else might find this usefull. But if yes, then use it like you want and like MIT license suggest.

Projects are being distributed as ordinary NuGet packages:
* [Jail](https://www.nuget.org/packages/Jail/), 
* [Jail.Design](https://www.nuget.org/packages/Jail.Design/), 
* [Jail.HelpersForTests](https://www.nuget.org/packages/Jail.HelpersForTests/).

Formerly these packages were known as one package [Cil](https://www.nuget.org/packages/Cil/).
One beatiful day its source code has been moved from [Bitbucket](https://bitbucket.org/hoborg91/cil) to [GitHub](https://github.com/hoborg91/jail) and splitted into parts (each part for different purpose).

## Functionality highlights

The library is splitted into three packages, each for a different purpose. Examples of the functionality provided by these packages are following.

### Jail (Common)

This library contains a range of additional types for general (`Set<T>`) and very specific (`MovingWindow<T>`) purposes and useful extension methods for common .NET types (`object`, `IEnumerable<T>`, etc.). Consider the following examples.

1. The `Set<T>` type is introduced because of absence of the `IReadOnlySet<T>` among the common .NET interfaces (there is such an interface in the `Microsoft.SqlServer.Management.Sdk.Sfc` namespace but its functionality differs from what I want). The `Set<T>` type is just a proxy over a common `HashSet<T>` implementing the new `IReadOnlySet<T>` interface. I like `IReadOnly*<T>` interfaces because they help to express the code writer intentions explicitly and they facilitate strong typing.
1. The `MovingWindow<T>` type (the default implementation of the new `IMovingWindow<T>` interface) is possibly useful in the mathematical and scientific-close applications. It represents a fragment of the collection, and one can change the fragment by "moving" the window forward over the collection.
1. The `T CheckArgumentNotNull<T>(this T[, string])` extension method generalizes the code snippets for "check and assign" and for "just check" operations on the input argument.
1. The `string JoinBy(this IEnumerable<string>, string)` extensions method simplifies the use of the standard `string.Join(string, IEnumerable<string>)` method.

### Jail.Design

The library contains types influencing the program design.

1. Use `IFileSystemApi` (and its default implementation `FileSystemApi`) to take advantage of the dependency inversion principle (DIP), when you do not want a code to strictly depend from the standard `System.IO` ecosystem. This interface facilitates the low coupling and improve testability.
1. The `IRailway` (and its default implementation `Railway`) is the core of provided micro-framework for "railway oriented programming". See [Railway Oriented Programming](https://fsharpforfunandprofit.com/rop/) (also available [in russian](https://habr.com/ru/post/339606/)) for reference.

### Jail.HelpersForTests

The library contains some classes which can help in unit (and possibly integration) testing. It's worth nothing that the techniques suggested by the library may be considered controversial, but they help in specific cases for sure.

The core functionality is located in the `UnitTestsHelper` which simplifies the construction of private classes of the assembly being tested. Also it can (more or less) automatically test if methods check their input parameters for null reference. Obviously this functionality became less usefull after the introducation of nullable reference types.
