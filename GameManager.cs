using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyNaiveGameEngine;

namespace MyNaiveGameEngine
{
    public class GameManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameIO _gameIO;
        public List<Type> AvailableGames { get; set; }

        /// <summary>
        /// Create a new game manager.
        /// serviceProvider and serviceCollection must contain games implementing
        /// IGame and all their needed dependencies.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="services">The service collection used to build serviceProvider.</param>
        public GameManager(IServiceProvider serviceProvider, IServiceCollection serviceCollection)
        {
            _serviceProvider = serviceProvider;
            _gameIO = serviceProvider.GetService<IGameIO>();
            // Enumerate all games using IGame interface.
            // Used to build selection list.
            AvailableGames = new List<Type>();
            foreach(var service in serviceCollection) {
                if(service.ImplementationType != null &&
                    service.ImplementationType.GetInterfaces().Contains(typeof(IGame)) )
                        AvailableGames.Add(service.ImplementationType);
            }
        }

        /// <summary>
        /// Handles selection and starting games using the IGame interface.
        /// If only one IGame has been added to services, no selection screen
        /// will be shown.
        /// </summary>
        public void Run(){
			while (true)
			{
                var game = SelectGame();
                if (game == null)
                    break;

                game.Run();

				_gameIO.WriteLine("Continue?");
				string answer = _gameIO.ReadLine();
				if (answer != null && answer != "" && answer.Substring(0, 1) == "n")
				    break;
			}
        }

        /// <summary>
        /// Used for selecting among multiple games.
        /// If only one game is available, returns that one immediately.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IGame? SelectGame() {
            if (AvailableGames.Count() < 1)
                throw new Exception("Could not find any games implementing interface 'IGame'.");
            if (AvailableGames.Count() == 1) {
                return (IGame?)_serviceProvider.GetService(AvailableGames.First());
            } else {
                var selection = _gameIO.ListSelect(AvailableGames);
                if (selection != null)
                    return (IGame?)_serviceProvider.GetService(selection);
                return null;
            }
        }
    }
}