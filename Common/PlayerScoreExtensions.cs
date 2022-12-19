using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{
    public static class PlayerScoreExtensions
    {
        public static List<PlayerData> ToToplist(this List<PlayerScore> playerScores)
        {
            var result = new List<PlayerData>();
            // Get list of all distinct player names.
            var playerNames = playerScores.Select(i => i.PlayerName).Distinct();

            foreach (var name in playerNames)
            {
                var scores = playerScores.Where(i => i.PlayerName == name);
                var gameCount = scores.Count();
                var totalGuessCount = scores.Sum(i => i.Score);

                var pd = new PlayerData(name, gameCount, totalGuessCount);
                result.Add(pd);
            }
            return result;
        }
    }
}