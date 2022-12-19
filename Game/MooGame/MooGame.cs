using Microsoft.Extensions.Configuration;

namespace MyNaiveGameEngine
{

    public partial class MooGame : IGame
    {
        // Settings. Move to GameState instead?
        // Create settings class and move to appsettings instead?
        private readonly string configSection = "MooGame";
        private readonly MooGameConfiguration _config = new MooGameConfiguration();
        private readonly IScoreStore _scoreStore;
        private readonly IGameIO _gameIO;
        private MooGameState state = new MooGameState();

        public MooGame(IGameIO gameIO, IScoreStore scoreStore, IConfiguration configuration)
        {
            _gameIO = gameIO;
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
            _gameIO.WriteLine("Enter your user name:\n");
			this.state.PlayerName = _gameIO.ReadLine();

            _gameIO.WriteLine("New game:\n");
            if(_config.PracticeMode)
                _gameIO.WriteLine("For practice, number is: " + state.Target + "\n");
            else
                _gameIO.WriteLine($"Guess the string. It contains {state.NumberOfCharactersInTarget} unique characters from '{state.AllowedCharacters}'.");

            // In the original code the first run does not echo back input.
            // That's why we have an initial round outside the while loop.
            var input = _gameIO.ReadLine();
            this.state.Guess(input);
            DisplayState();
        }

        /// <summary>
        /// Called every step other than first or last.
        /// </summary>
        public void Step()
        {
            var input = _gameIO.ReadLine();
            _gameIO.WriteLine(input + "\n");
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

            _gameIO.WriteLine("Correct, it took " + state.TryCountOnFirstSuccess + " guesses");
        }

        /// <summary>
        /// Initialize a new game.
        /// </summary>
        public void Initialize()
        {
            // Note, Initialize() clears all state.
            this.state = new MooGameState(_config.AllowedCharacters, _config.NumberOfCharactersInTarget);
            return;
        }

        public void LoadState(IGameState state)
        {
            if (state is MooGameState)
                this.state = new MooGameState((MooGameState)state);
            else
                throw new ArgumentException($"'state' should be of type {typeof(MooGameState)}");
        }
    }
}