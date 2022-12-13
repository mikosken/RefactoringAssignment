using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{
    public class BullsAndCowsGameState
    {

        // See property Guesses for external access.
        private List<string> guesses = new List<string>();
        
        public string Target { get; set; } = "";

        /// <summary>
        /// Initializes a new game of Bulls and Cows.
        /// </summary>
        public BullsAndCowsGameState()
        {
        }

        /// <summary>
        /// Use this constructor to load an existing gamestate.
        /// </summary>
        /// <param name="state"></param>
        public BullsAndCowsGameState(BullsAndCowsGameState state)
        {
            Target = state.Target;
            this.guesses = state.Guesses.ToList();
        }


        /// <summary>
        /// True if end condition has been met.
        /// </summary>
        public bool Success
        {
            get
            {
                return Guesses.Contains(Target);
            }
        }

        /// <summary>
        /// Number of tries until first correct guess (in case you somehow
        /// were allowed to continue after getting a correct guess).
        /// 0 if no correct guess has been made.
        /// </summary>
        public int TryCountOnFirstSuccess
        {
            get
            {
                var triesBeforeCorrect = Guesses.TakeWhile(t => !(t == Target)).Count();
                return triesBeforeCorrect + 1;
            }
        }

        public IReadOnlyList<string> Guesses
        {
            get
            {
                return this.guesses.AsReadOnly();
            }
        }

        /// <summary>
        /// Attempt a new guess.
        /// </summary>
        /// <param name="guess"></param>
        /// <returns>True if guess succeeded.</returns>
        public bool Guess(string guess) {
            this.guesses.Add(guess);
            return Success;
        }
    }

    public class BullsAndCowsGame : IGame<BullsAndCowsGameState>
    {
        private readonly string allowedCharacters = "1234567890";
        private readonly int numberOfCharactersInTarget = 4;
        private readonly int maxCharactersInInput = 4;

        private BullsAndCowsGameState state = new BullsAndCowsGameState();
        private string latestInput = "";

        public BullsAndCowsGame()
        {
        }

        /// <summary>
        /// BullsAndCowsGame only tracks latest input.
        /// To overwrite guess just call again before calling Step().
        /// </summary>
        /// <param name="input"></param>
        public void AddInput(string input)
        {
            latestInput = (input ?? "").PadRight(maxCharactersInInput).Substring(0,maxCharactersInInput);
        }

        BullsAndCowsGameState IGame<BullsAndCowsGameState>.GetState()
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
        void IGame<BullsAndCowsGameState>.Initialize()
        {
            // Note, Initialize() clears all state.
            this.state = new BullsAndCowsGameState();

            // Split the string with allowed characters into a hashset.
            var allowedItems = allowedCharacters.Select(i => i.ToString()).ToHashSet();
            var randomizedItems = Helpers.RandomSelection(numberOfCharactersInTarget, allowedItems) ?? new List<string>();
            this.state.Target = string.Join("", randomizedItems);
            return;
        }

        void IGame<BullsAndCowsGameState>.LoadState(BullsAndCowsGameState state)
        {
            this.state = new BullsAndCowsGameState(state);
        }

        // MOVE THIS TO A DISPLAY MANAGER FOR BULLS AND COWS GAME
        public string GuessResultAsString() {
            var target = this.state.Target ?? "";
            var latestGuess = this.state.Guesses.LastOrDefault() ?? "";

            var correctItemCorrectPlace = 0;
            var correctItemWrongPlace = 0;

            for (int i = 0; i < latestGuess.Count(); i++) {
                if (i < target.Count() && target[i] == latestGuess[i]) {
                    correctItemCorrectPlace += 1;
                } else {
                    correctItemWrongPlace += target.Contains(latestGuess[i]) ? 1 : 0;
                }
            }
            var bulls = new String('B', correctItemCorrectPlace);
            var cows = new String('C', correctItemWrongPlace);
            return $"{bulls},{cows}";
        }
    }
}