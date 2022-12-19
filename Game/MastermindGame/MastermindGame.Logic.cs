namespace MyNaiveGameEngine
{
    public partial class MastermindGame
    {
        private void SetUsername() {
            _gameIO.WriteLine("Enter your user name:\n");
			this.state.PlayerName = _gameIO.ReadLine();
        }

        private void DisplayStartupInfo() {
            _gameIO.WriteLine("New game:\n");
            if(_config.PracticeMode)
                _gameIO.WriteLine("For practice, solution is: " + state.Target + "\n");
            else
                _gameIO.WriteLine($"Guess the string. It contains {state.NumberOfCharactersInTarget} unique characters from '{state.AllowedCharacters}'.");
        }

        private void DisplayState()
        {
            _gameIO.WriteLine(this.state.ToString() + "\n");
        }

        private void DisplayToplist() {
            _scoreStore.LoadScores(_config.ScoreFile);
            var toplist = _scoreStore.Scores.ToToplist();

            toplist.Sort((p1, p2) => p1.Average().CompareTo(p2.Average()));
			_gameIO.WriteLine("Player   games average");
			foreach (PlayerData pd in toplist)
			{
                _gameIO.WriteLine(pd.ToString("{NAME,-9}{GAMECOUNT,5:D}{AVERAGE,9:F2}"));
			}
        }

        private void SaveScore() {
            _scoreStore.LoadScores(_config.ScoreFile);
            var playerScore = new PlayerScore(this.state.PlayerName, this.state.TryCountOnFirstSuccess);
            _scoreStore.AddScore(playerScore);
            _scoreStore.SaveScores(_config.ScoreFile);
        }
    }
}