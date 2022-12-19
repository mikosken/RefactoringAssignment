using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MyNaiveGameEngine;
using Microsoft.Extensions.Configuration;

namespace Game
{
	class MainClass
	{

		public static void Main(string[] args)
		{

            // Setup configuration and inject dependencies.
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false)
                .Build();
            
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IConsoleIO, ConsoleIO>()
                .AddTransient<IScoreStore, FileScoreStore>()
                .AddTransient<MooGame>()
                .BuildServiceProvider();

			bool playOn = true;

			while (playOn)
			{
                var game = serviceProvider.GetService<MooGame>();
                game.Run();
                var nGuess = ((MooGameState)game.GetState()).TryCountOnFirstSuccess;

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