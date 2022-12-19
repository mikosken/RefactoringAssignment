namespace MyNaiveGameEngine
{
    public class PlayerScore
    {
        public PlayerScore(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
        }

        public string PlayerName { get; set; } = "";
        public int Score { get; set; }
    }
}