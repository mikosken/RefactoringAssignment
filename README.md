# Refactoring Assignment

The code in this repository is based on an assignment in refactoring and code smells.

## Refactoring changes and reasoning

### Notes on original code.

#### An example run

My comments are preceeded by #

```
Enter your user name:

John Smith
New game:

For practice, number is: 3178

2       # First input.
,       # Nothing correct.

1234    # Second input
1234    # Note that after the first input the input is echoed back.

,CC     # Two numbers correct, in wrong places.

1111
1111

B,CCC   # One of the 1 is in the correct spot.

3178
3178

BBBB,

Player   games average
John Smith    1     4,00
Correct, it took 4 guesses
Continue?
n
```

### Game manager

A game manager was implemented to handle selection of games, if multiple games
are implemented, and for restarting games if the player wishes to continue.

### Game Class and Game Loop

Originally the game logic was intertwined with the game loop.

A game usually has:

-   a state
-   accepts/requests input
-   continues from one state to the next

Displaying the state and retrieving input should be separated from the main game
logic using dependency injection.

By putting the game logic into it's own class with a defined interface, `IGame`,
a new game can easily be instantiated, and it can also be swapped with a
different game using the same interface. By being able to swap different games
with the same interface it could be considered to use the Strategy design
pattern.

The game class was then split into two partial classes.

-   `BullsAndCowsGame.cs` contains very abstract and high level code
    as defined by the `IGame` interface.
-   `BullsAndCowsGame.Logic.cs` contains slightly less abstract code dealing
    with supporting classes and services.

### Game settings

Game settings were implemented using `Microsoft.Extensions.Configuration`,
which enables injecting `IConfiguration`.
The game classes contain default values for properties, but these are overridden
by the settings in `appSettings.json`.

### Game IO / Wrapping System.Console

To make console input and output testable, as well as enable dependency
injection of the console IO, a wrapper interface was created: `IConsoleIO`.

### File handling / high score handling

As the existing game already had a set file format for saving scores, the format
was kept for compatibility purposes. Although if compatibility wasn't an issue
it would likely be easier to replace with JSON files, or with a small DB and use
Entity Framework.

The result is an interface for saving/loading scores `IScoreStore`.
`IScoreStore` is implemented as `FileScoreStore`, that reads and writes a
file.

The original class `PlayerData` was moved to a separate file. The original
funtionality was kept but some properties were renamed and the field `totalGuess`
was replaced by the property `TotalGuessCount`, and an additional constructor
was added to directly initialize the properties.
`ToString(string format)` was also added for convenience.

To calculate statistics in the form of a list of `PlayerData`, an extension
method called `ToToplist()` for `List<PlayerScore>` was added to a static class
called `ToplistExtensions`.

### Dependency Injection

The core of Dependency Injection is that each class should only be concerned
with implementing one specific thing, and any secondary/external functionality
it needs should be **injected** and implemented elsewhere.

In dotnet this is usually accomplished by using Constructor Injection, where
all required dependencies are injected in the constructor on creation.

Where do the dependencies come from at runtime then? The dependencies are
usually specified early in the `Main()` method, or in a startup method called
from `Main()`.

A standard approach is to use the `Microsoft.Extensions.DependencyInjection`
package, create a service provider, and add all dependencies to the service
provider. The service provider then takes care of handling which dependency
goes where.

Dependencies are added with differing scopes:

-   Singleton: After initializing the requested dependency once it's kept in
    memory and reused until the application shuts down.
-   Scoped: A new instance of the dependency is created every time a new scope
    is created. (Create new scope, get new service provider from scope, get
    dependency from service provider.)
-   Transient: A new instance of the dependency is created every time that
    dependency is requested.

## Getting Started

To try out this project or continue development try this:

Install **git**, **dotnet SDK**, and **Visual Studio Code** and then open a
console:

```
cd .\suitable\project\folder
git clone <address_to_this_repo>
cd ".\RefactoringAssignment"
code .
```

To build and run enter in console:

```
dotnet build
dotnet watch run
```

## References

Inspiration, code snippets, etc.

-   [Example](https://example.com/)
-   [Using dependency injection in a dotNet Core Console App](https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/)
-   [C# - How to unit test code that reads and writes to the console](https://makolyte.com/csharp-how-to-unit-test-code-that-reads-and-writes-to-the-console/)
