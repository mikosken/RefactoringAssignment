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
            
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IGameIO, ConsoleGameIO>()
                .AddTransient<IScoreStore, FileScoreStore>()
                .AddTransient<MooGame>();
            var serviceProvider = services
                .BuildServiceProvider();

            var gameManager = new GameManager(serviceProvider, services);
            gameManager.Run();
		}
	}
}