using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{

    public class BullsAndCowsGame : IGame
    {
        // Settings. Move to GameState instead?
        // Create settings class and move to appsettings instead?
        private readonly string scoreFile = "result.txt";
        private readonly IScoreStore _scoreStore;
        private readonly IConsoleIO _consoleIO;
        private BullsAndCowsGameState state = new BullsAndCowsGameState();
        private string latestInput = "";

        public BullsAndCowsGame(IConsoleIO consoleIO, IScoreStore scoreStore)
        {
            _consoleIO = consoleIO;
            _scoreStore = scoreStore;
        }

        /// <summary>
        /// BullsAndCowsGame only tracks latest input.
        /// To overwrite guess just call again before calling Step().
        /// </summary>
        /// <param name="input"></param>
        public void AddInput(string input)
        {
            latestInput = input;
        }

        public IGameState GetState()
        {
            return this.state;
        }

        /// <summary>
        /// Passes any input to internal gamestate and updates gamestate.
        /// </summary>
        public void Step()
        {
            this.state.Guess(latestInput);
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

        public void GetInput()
        {
            this.latestInput = _consoleIO.ReadLine();
        }

        public void DisplayState()
        {
            _consoleIO.WriteLine(this.state.ToString() + "\n");
        }

        public void SaveScore() {
            _scoreStore.LoadScores(scoreFile);
            var playerScore = new PlayerScore(this.state.PlayerName, this.state.TryCountOnFirstSuccess);
            _scoreStore.AddScore(playerScore);
            _scoreStore.SaveScores(scoreFile);
        }
        public void DisplayToplist() {
            _scoreStore.LoadScores(scoreFile);
            var toplist = _scoreStore.Scores.ToToplist();

            toplist.Sort((p1, p2) => p1.Average().CompareTo(p2.Average()));
			Console.WriteLine("Player   games average");
			foreach (PlayerData pd in toplist)
			{
                _consoleIO.WriteLine(pd.ToString("{NAME,-9}{GAMECOUNT,5:D}{AVERAGE,9:F2}"));
			}
        }

        public void Run()
        {
            Console.WriteLine("Enter your user name:\n");
			this.state.PlayerName = _consoleIO.ReadLine();

            _consoleIO.WriteLine("New game:\n");
            //comment out or remove next line to play real games!
            _consoleIO.WriteLine("For practice, number is: " + state.Target + "\n");
            // In the original code the first run does not echo back input.
            // That's why we have an initial round outside the while loop.
            GetInput();
            Step();
            DisplayState();

            while (!this.state.Success)
            {
                GetInput();
                _consoleIO.WriteLine(latestInput + "\n");
                Step();
                DisplayState();
            }

            SaveScore();
            DisplayToplist();
        }

        public void DisplayHighscore()
        {
            throw new NotImplementedException();
        }
    }
}