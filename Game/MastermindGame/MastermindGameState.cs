namespace MyNaiveGameEngine
{
    public class MastermindGameState : IGameState
    {
        public string AllowedCharacters { get; private set; } = "123456";
        public int NumberOfCharactersInTarget { get; private set; } = 4;

        // See property Guesses for external access.
        private List<string> guesses = new List<string>();
        
        public string Target { get; set; } = "";

        public string PlayerName { get; set; } = "";

        /// <summary>
        /// Initializes a new game of Bulls and Cows.
        /// </summary>
        public MastermindGameState()
        {
            GenerateTarget();
        }

        /// <summary>
        /// Initializes a new game of mastermind with custom settings.
        /// </summary>
        public MastermindGameState(string allowedCharacters, int numberOfCharactersInTarget)
        {
            AllowedCharacters = allowedCharacters;
            NumberOfCharactersInTarget = numberOfCharactersInTarget;
            GenerateTarget();
        }

        /// <summary>
        /// Use this constructor to load an existing gamestate.
        /// </summary>
        /// <param name="state"></param>
        public MastermindGameState(MastermindGameState state)
        {
            AllowedCharacters = state.AllowedCharacters;
            NumberOfCharactersInTarget = state.NumberOfCharactersInTarget;
            PlayerName = state.PlayerName;
            Target = state.Target;
            this.guesses = state.Guesses.ToList();
        }

        public override string ToString()
        {
            (var fullyCorrect, var partiallyCorrect) = CheckCorrect();
            return $"{fullyCorrect} right character and place, {partiallyCorrect} right character.";
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
                if (Success) {
                    var triesBeforeCorrect = Guesses.TakeWhile(t => !(t == Target)).Count();
                    return triesBeforeCorrect + 1;
                }
                return 0;
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
            var paddedGuess = (guess ?? "").PadRight(NumberOfCharactersInTarget).Substring(0, NumberOfCharactersInTarget);
            this.guesses.Add(paddedGuess);
            return Success;
        }

        /// <summary>
        /// Generates and sets a new target string.
        /// </summary>
        public void GenerateTarget(){
            // Split the string with allowed characters into a hashset.
            var allowedItems = AllowedCharacters.Select(i => i.ToString()).ToHashSet();
            var randomizedItems = Helpers.RandomSelection(NumberOfCharactersInTarget, allowedItems, true) ?? new List<string>();
            Target = string.Join("", randomizedItems);
        }

        /// <summary>
        /// Checks how many characters in latest guess are correct and in the
        /// correct spot, or correct and in the wrong spot.
        /// </summary>
        /// <returns>A tuple of (int FullyCorrect, int PartiallyCorrect)</returns>
        public Tuple<int, int> CheckCorrect()
        {
            var guess = this.Guesses.LastOrDefault() ?? "";
            var target = this.Target;
            return CheckCorrect(guess, target);
        }
        
        /// <summary>
        /// Checks how many characters in latest guess are correct and in the
        /// correct spot, or correct and in the wrong spot.
        /// </summary>
        /// <returns>A tuple of (int FullyCorrect, int PartiallyCorrect)</returns>
        public Tuple<int, int> CheckCorrect(string guess, string target) {
            var correctItemCorrectPlace = 0;
            var correctItemWrongPlace = 0;

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