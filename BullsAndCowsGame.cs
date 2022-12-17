using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{

    public class BullsAndCowsGame : IGame
    {
        private readonly string allowedCharacters = "1234567890";
        private readonly int numberOfCharactersInTarget = 4;
        private readonly int maxCharactersInInput = 4;
        private readonly IConsoleIO _consoleIO;
        private BullsAndCowsGameState state = new BullsAndCowsGameState();
        private string latestInput = "";

        public BullsAndCowsGame(IConsoleIO consoleIO)
        {
            _consoleIO = consoleIO;
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

        public IGameState GetState()
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
        public void Initialize()
        {
            // Note, Initialize() clears all state.
            this.state = new BullsAndCowsGameState();

            // Split the string with allowed characters into a hashset.
            var allowedItems = allowedCharacters.Select(i => i.ToString()).ToHashSet();
            var randomizedItems = Helpers.RandomSelection(numberOfCharactersInTarget, allowedItems) ?? new List<string>();
            this.state.Target = string.Join("", randomizedItems);
            return;
        }

        public void LoadState(IGameState state)
        {
            if (state is BullsAndCowsGameState)
                this.state = new BullsAndCowsGameState((BullsAndCowsGameState)state);
            else
                throw new ArgumentException($"'state' should be of type {typeof(BullsAndCowsGameState)}");
        }

        public void GetInput()
        {
            this.latestInput = _consoleIO.ReadLine();
        }

        public void DisplayState()
        {
            _consoleIO.WriteLine(this.state.ToString() + "\n");
        }

        public void Run()
        {
            _consoleIO.WriteLine("New game:\n");
            //comment out or remove next line to play real games!
            _consoleIO.WriteLine("For practice, number is: " + state.Target + "\n");
            // In the original code the first run does not echo back input.
            // That's why we have an initial round outside the while loop.
            (this as IGame).GetInput();
            (this as IGame).Step();
            (this as IGame).DisplayState();

            while (!this.state.Success)
            {
                (this as IGame).GetInput();
                _consoleIO.WriteLine(latestInput + "\n");
                (this as IGame).Step();
                (this as IGame).DisplayState();
            }
        }
    }
}