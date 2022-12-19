using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{

    public partial class BullsAndCowsGame : IGame
    {
        // Settings. Move to GameState instead?
        // Create settings class and move to appsettings instead?
        private readonly string scoreFile = "result.txt";
        private readonly IScoreStore _scoreStore;
        private readonly IConsoleIO _consoleIO;
        private readonly IGameIO _gameIO;
        private BullsAndCowsGameState state = new BullsAndCowsGameState();
        private string latestInput = "";

        public BullsAndCowsGame(IConsoleIO consoleIO, IScoreStore scoreStore)
        {
            _consoleIO = consoleIO;
            _scoreStore = scoreStore;
        }

        public IGameState GetState()
        {
            return this.state;
        }

        /// <summary>
        /// Called once before main loop of game steps.
        /// </summary>
        public void FirstStep()
        {
            _consoleIO.WriteLine("Enter your user name:\n");
			this.state.PlayerName = _consoleIO.ReadLine();

            _consoleIO.WriteLine("New game:\n");
            //comment out or remove next line to play real games!
            _consoleIO.WriteLine("For practice, number is: " + state.Target + "\n");

            this.latestInput = _consoleIO.ReadLine();
            // In the original code the first run does not echo back input.
            // That's why we have an initial round outside the while loop.
            this.state.Guess(latestInput);
            DisplayState();
        }

        /// <summary>
        /// Called every step other than first or last.
        /// </summary>
        public void Step()
        {
            this.latestInput = _consoleIO.ReadLine();
            _consoleIO.WriteLine(latestInput + "\n");
            this.state.Guess(latestInput);
            DisplayState();
        }

        /// <summary>
        /// Called once after main loop of game steps.
        /// </summary>
        public void LastStep()
        {
            SaveScore();
            DisplayToplist();
        }

        /// <summary>
        /// Initialize a new game.
        /// </summary>
        public void Initialize()
        {
            // Note, Initialize() clears all state.
            this.state = new BullsAndCowsGameState();
            this.state.GenerateTarget();
            return;
        }

        public void LoadState(IGameState state)
        {
            if (state is BullsAndCowsGameState)
                this.state = new BullsAndCowsGameState((BullsAndCowsGameState)state);
            else
                throw new ArgumentException($"'state' should be of type {typeof(BullsAndCowsGameState)}");
        }

        /// <summary>
        /// Main game loop.
        /// </summary>
        public void Run()
        {
            FirstStep();
            while (!this.state.Success)
            {
                Step();
            }
            LastStep();
        }
    }
}