using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MyNaiveGameEngine;

namespace MooGame
{
	class MainClass
	{

		public static void Main(string[] args)
		{

            // Setup Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConsoleIO, ConsoleIO>()
                .AddTransient<IScoreStore, FileScoreStore>()
                .AddTransient<BullsAndCowsGame>()
                .BuildServiceProvider();

			bool playOn = true;
			// Console.WriteLine("Enter your user name:\n");
			// string name = Console.ReadLine();

			while (playOn)
			{
                var game = serviceProvider.GetService<BullsAndCowsGame>();
                game.Initialize();
                game.Run();
                var nGuess = ((BullsAndCowsGameState)game.GetState()).TryCountOnFirstSuccess;

                // Save result, display toplist, check if run again.
                // StreamWriter output = new StreamWriter("result.txt", append: true);
				// output.WriteLine(name + "#&#" + nGuess);
				// output.Close();
				//showTopList();
				Console.WriteLine("Correct, it took " + nGuess + " guesses\nContinue?");
				string answer = Console.ReadLine();
				if (answer != null && answer != "" && answer.Substring(0, 1) == "n")
				{
					playOn = false;
				}
			}
		}
	}
}