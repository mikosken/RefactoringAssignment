namespace MyNaiveGameEngine
{
    public partial class MooGame
    {
        public void DisplayState()
        {
            _consoleIO.WriteLine(this.state.ToString() + "\n");
        }

        private void DisplayToplist() {
            _scoreStore.LoadScores(_config.ScoreFile);
            var toplist = _scoreStore.Scores.ToToplist();

            toplist.Sort((p1, p2) => p1.Average().CompareTo(p2.Average()));
			Console.WriteLine("Player   games average");
			foreach (PlayerData pd in toplist)
			{
                _consoleIO.WriteLine(pd.ToString("{NAME,-9}{GAMECOUNT,5:D}{AVERAGE,9:F2}"));
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