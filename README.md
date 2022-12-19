# Refactoring Assignment

The code in this repository is based on an assignment in refactoring and code smells.

## Refactoring changes and reasoning

### Files and folders.

The solution is split into two projects, Game and GameTest.

The Game code was split in four separate subfolders:

-   Interfaces
-   Common: For the game manager, and classes that are likely to be shared
    between multiple games.
-   MooGame: The refactored game.
-   MastermindGame: A modified implementation of MooGame, but still using the
    same interfaces.

### Dependency Injection

The core of Dependency Injection is that each class should only be concerned
with implementing one specific thing, and any secondary/external functionality
it needs should be injected and implemented elsewhere.

In C#/dotNet this is usually accomplished by using Constructor Injection, where
all required dependencies are injected in the constructor on creation.

A standard approach is to use the `Microsoft.Extensions.DependencyInjection`
package, create a service provider, and add all dependencies to the service
provider. The service provider then takes care of handling which dependency
goes where.

### Game manager

A very simple game manager was implemented to handle selection of games, if
multiple games are implemented, and for restarting games if the player wishes to
continue.

The player is prompted to select between games at startup and after a completed
round of the selected game.
If only one game is implemented it is started immediately without prompting the
user for a choice.

### Game Class and Game Loop

Originally the game logic was intertwined with the program logic and flow
control, but through refactoring the main game logic, game state, Input/Output,
File handling, and game management was separated.

By putting the game logic into it's own class with a defined interface, `IGame`,
a new game can easily be instantiated, and other games can be implemented
using the same interface.

The game class was also split into two partial classes.

-   `MooGame.cs` contains more abstract and slightly higher level code
    as defined by the `IGame` interface.
-   `MooGame.Logic.cs` contains slightly less abstract code dealing with
    internal game logic and display.

### Game settings

Game settings were implemented using `Microsoft.Extensions.Configuration`,
which enables injecting `IConfiguration`.
The game classes contain default values for properties, but these are overridden
by the settings in `appSettings.json`.

To switch the games from practice mode to live mode, open `appSettings.json` and
change the setting "PracticeMode" to false.

It's also possible to adjust the difficulty of the games in settings by changing
allowed characters and length of target string.

### Game IO / Wrapping System.Console

To make console input and output testable, as well as enable dependency
injection of the console IO, a wrapper interface was created: `IGameIO`.
This interface was then implemented as `ConsoleGameIO`.

### File handling / high score handling

As the existing game already had a set file format for saving scores, the format
was kept for compatibility purposes. Although if compatibility wasn't an issue
it would likely be easier to replace with JSON files, or with a small DB and use
Entity Framework.

The result is an interface for saving/loading scores `IScoreStore`.
`IScoreStore` is implemented as `FileScoreStore`, that reads and writes to a
text file.

The original class `PlayerData` was moved to a separate file. The original
funtionality was kept, but some properties were renamed and the field `totalGuess`
was replaced by the property `TotalGuessCount`, and an additional constructor
was added to directly initialize the properties.
`ToString(string format)` was also added for convenience.

To calculate statistics in the form of a list of `PlayerData`, an extension
method called `ToToplist()` for `List<PlayerScore>` was added to a static class
called `ToplistExtensions`.

### Testing

A separate project for testing using the mstest package was created, called
`GameTest`.

To enable testing of MooGame mock implementations were required for IGameIO and
IScoreStore. In-memory configuration was used to enable specifying
appsettings during test initialization.

Tests were created for MooGame, MooGameState, PlayerData, and PlayerScore.

### Implementing new games

By having a well defined game interface, and a game manager that automatically
detects new injected games, it's easy to add new games.

1. Create a new subfolder for the game to keep the files separate.
2. Implement IGame/IGameState, a configuration class if needed and add to
   appsettings.
3. Inject new game implementation in Program.cs.
4. Done!

Using this methodology MastermindGame was implemented with nearly identical
game logic as MooGame, but target generation was changed to allow repeated
characters.

### An example run

Here is an example run of the original program, and the refactored version
is functionally indistinguishable when only MooGame is injected in program.cs.
If more games are injected then a select menu appears between game rounds.

My comments are preceeded by #.

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

-   [Using dependency injection in a dotNet Core Console App](https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/)
-   [C# - How to unit test code that reads and writes to the console](https://makolyte.com/csharp-how-to-unit-test-code-that-reads-and-writes-to-the-console/)
-   [Test a .NET class library using Visual Studio Code](https://learn.microsoft.com/en-us/dotnet/core/tutorials/testing-library-with-visual-studio-code?pivots=dotnet-7-0)
