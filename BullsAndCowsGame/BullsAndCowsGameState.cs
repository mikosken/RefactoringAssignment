namespace MyNaiveGameEngine
{
    public class BullsAndCowsGameState : IGameState
    {
        private readonly string allowedCharacters = "1234567890";
        private readonly int numberOfCharactersInTarget = 4;

        // See property Guesses for external access.
        private List<string> guesses = new List<string>();
        
        public string Target { get; set; } = "";

        public string PlayerName { get; set; } = "";

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

        public override string ToString()
        {
            (var bulls, var cows) = CheckBullsAndCows();
            var b = new String('B', bulls);
            var c = new String('C', cows);
            return $"{b},{c}";
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
            var paddedGuess = (guess ?? "").PadRight(numberOfCharactersInTarget).Substring(0, numberOfCharactersInTarget);
            this.guesses.Add(paddedGuess);
            return Success;
        }

        /// <summary>
        /// Generates and sets a new target string.
        /// </summary>
        public void GenerateTarget(){
            // Split the string with allowed characters into a hashset.
            var allowedItems = allowedCharacters.Select(i => i.ToString()).ToHashSet();
            var randomizedItems = Helpers.RandomSelection(numberOfCharactersInTarget, allowedItems) ?? new List<string>();
            Target = string.Join("", randomizedItems);
        }

        /// <summary>
        /// Checks how many characters in latest guess are
        /// bulls (Correct character, in correct place), and
        /// cows (Correct character, in wrong place).
        /// </summary>
        /// <returns>A tuple of (int Bulls, int Cows)</returns>
        public Tuple<int, int> CheckBullsAndCows()
        {
            var guess = this.Guesses.LastOrDefault() ?? "";
            var target = this.Target;
            return CheckBullsAndCows(guess, target);
        }
        
        /// <summary>
        /// Checks how many characters in supplied guess and target are
        /// bulls (Correct character, in correct place), and
        /// cows (Correct character, in wrong place).
        /// </summary>
        /// <returns>A tuple of (int Bulls, int Cows)</returns>
        public Tuple<int, int> CheckBullsAndCows(string guess, string target) {
            var correctItemCorrectPlace = 0; // Bulls.
            var correctItemWrongPlace = 0; // Cows.

            for (int i = 0; i < guess.Count(); i++) {
                if (i < target.Count() && target[i] == guess[i]) {
                    correctItemCorrectPlace += 1;
                } else {
                    correctItemWrongPlace += target.Contains(guess[i]) ? 1 : 0;
                }
            }
            return Tuple.Create(correctItemCorrectPlace, correctItemWrongPlace);
        }

    }
}