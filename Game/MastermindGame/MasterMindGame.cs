using Microsoft.Extensions.Configuration;

namespace MyNaiveGameEngine
{

    public partial class MastermindGame : IGame
    {
        private readonly string configSection = "MastermindGame";
        private readonly MastermindGameConfiguration _config = new MastermindGameConfiguration();
        private readonly IScoreStore _scoreStore;
        private readonly IGameIO _gameIO;
        private MastermindGameState state = new MastermindGameState();

        public MastermindGame(IGameIO gameIO, IScoreStore scoreStore, IConfiguration configuration)
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
            SetUsername();
            DisplayStartupInfo();

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
            this.state = new MastermindGameState(_config.AllowedCharacters, _config.NumberOfCharactersInTarget);
            return;
        }

        public void LoadState(IGameState state)
        {
            if (state is MastermindGameState)
                this.state = new MastermindGameState((MastermindGameState)state);
            else
                throw new ArgumentException($"'state' should be of type {typeof(MastermindGameState)}");
        }
    }
}