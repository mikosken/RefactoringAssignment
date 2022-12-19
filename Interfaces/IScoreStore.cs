using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{
    public interface IScoreStore
    {
        List<PlayerScore> Scores { get; }

        /// <summary>
        /// Adds a new score to the score list.
        /// Don't forget to call <c>SaveScores</c> to persist changes.
        /// </summary>
        /// <param name="playerScore"></param>
        void AddScore(PlayerScore playerScore);

        /// <summary>
        /// Saves the score to the store identified by <c>storage</c>.
        /// Depending on implementation it might be saved to memory,
        /// a database, file, etc.
        /// </summary>
        /// <param name="storage">Identifier for storage.</param>
        /// <param name="player">Name of the player.</param>
        /// <param name="score">Score to save</param>
        void SaveScores(string storage);

        /// <summary>
        /// Loads previously saved scores.
        /// </summary>
        /// <param name="storage">Identifier for storage.</param>
        void LoadScores(string storage);
    }
}