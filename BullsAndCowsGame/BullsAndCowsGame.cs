using Microsoft.Extensions.Configuration;

namespace MyNaiveGameEngine
{

    public partial class BullsAndCowsGame : IGame
    {
        // Settings. Move to GameState instead?
        // Create settings class and move to appsettings instead?
        private readonly string configSection = "MooGame";
        private readonly BullsAndCowsGameConfiguration _config = new BullsAndCowsGameConfiguration();
        private readonly IScoreStore _scoreStore;
        private readonly IConsoleIO _consoleIO;
        private BullsAndCowsGameState state = new BullsAndCowsGameState();

        public BullsAndCowsGame(IConsoleIO consoleIO, IScoreStore scoreStore, IConfiguration configuration)
        {
            _consoleIO = consoleIO;
            _scoreStore = scoreStore;
            configuration.GetSection(configSection).Bind(_config);

            Initialize();
        }

        public IGameState GetState()
        {
            return this.state;
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

        /// <summary>
        /// Called once before main loop.
        /// </summary>
        public void FirstStep()
        {
            _consoleIO.WriteLine("Enter your user name:\n");
			this.state.PlayerName = _consoleIO.ReadLine();

            _consoleIO.WriteLine("New game:\n");
            if(_config.PracticeMode)
                _consoleIO.WriteLine("For practice, number is: " + state.Target + "\n");
            else
                _consoleIO.WriteLine($"Guess the string. It contains {state.NumberOfCharactersInTarget} unique characters from '{state.AllowedCharacters}'.");

            // In the original code the first run does not echo back input.
            // That's why we have an initial round outside the while loop.
            var input = _consoleIO.ReadLine();
            this.state.Guess(input);
            DisplayState();
        }

        /// <summary>
        /// Called every step other than first or last.
        /// </summary>
        public void Step()
        {
            var input = _consoleIO.ReadLine();
            _consoleIO.WriteLine(input + "\n");
            this.state.Guess(input);
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
            this.state = new BullsAndCowsGameState(_config.AllowedCharacters, _config.NumberOfCharactersInTarget);
            return;
        }

        public void LoadState(IGameState state)
        {
            if (state is BullsAndCowsGameState)
                this.state = new BullsAndCowsGameState((BullsAndCowsGameState)state);
            else
                throw new ArgumentException($"'state' should be of type {typeof(BullsAndCowsGameState)}");
        }
    }
}