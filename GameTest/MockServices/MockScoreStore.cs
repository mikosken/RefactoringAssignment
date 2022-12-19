using System.Collections.Generic;
using MyNaiveGameEngine;

namespace GameTest.MockServices
{
    public class MockScoreStore : IScoreStore
    {
        public List<PlayerScore> Scores { get; set; }

        public void AddScore(PlayerScore playerScore)
        {
            Scores.Add(playerScore);
        }

        public void LoadScores(string storage)
        {
            Scores = new List<PlayerScore>();
            Scores.Add(new PlayerScore("Testplayer1", 2));
            Scores.Add(new PlayerScore("Testplayer2", 1));
            Scores.Add(new PlayerScore("Testplayer1", 4));
            Scores.Add(new PlayerScore("Testplayer3", 2));
        }

        public void SaveScores(string storage)
        {
            return;
        }
    }
}