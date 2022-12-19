using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{
    public class FileScoreStore : IScoreStore
    {
        private readonly string FileValueSeparator = "#&#";
        public List<PlayerScore> Scores { get; private set; } = new List<PlayerScore>();

        public void AddScore(PlayerScore playerScore)
        {
            Scores.Add(playerScore);
        }

        public void SaveScores(string storage)
        {
            using (var fileStream = new StreamWriter(storage, false))
            {
                foreach(var score in Scores) {
                    fileStream.WriteLine($"{score.PlayerName}{FileValueSeparator}{score.Score}");
                }
            }
        }

        public void LoadScores(string storage)
        {
            if (File.Exists(storage)) {
                using (var fileStream = new StreamReader(storage))
                {
                    var loadedScores = new List<PlayerScore>();
                    string? line;

                    while((line = fileStream.ReadLine()) != null) {
                        string[] nameAndScore = line.Split(new string[] { "#&#" }, StringSplitOptions.None);
                        // Sanity check.
                        if (nameAndScore.Length == 2
                            && int.TryParse(nameAndScore[1], out int score))
                        {
                            var ps = new PlayerScore(nameAndScore[0], score);
                            loadedScores.Add(ps);
                        }
                    }
                    Scores = loadedScores;
                }
            }
        }

    }
}