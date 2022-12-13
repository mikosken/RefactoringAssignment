namespace MyNaiveGameEngine
{
    public interface IGame<T>
    {
        /// <summary>
        /// This is called first before anything else in your game.
        /// Place initialization code here.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Load an existing gamestate.
        /// </summary>
        /// <param name="state"></param>
        void LoadState(T state);

        /// <summary>
        /// Run the game for one game step, changes game state.
        /// Always call after giving input to game to see effects.
        /// </summary>
        void Step();

        /// <summary>
        /// Add new input. It is up to the game implementation to determine if
        /// multiple inputs can be queued, or if only the last input is used
        /// upon Step().
        /// </summary>
        /// <param name="input"></param>
        void AddInput(string input);

        /// <summary>
        /// Returns the current state of the game. It is up to the
        /// displayManager to figure out how to display it.
        /// </summary>
        /// <returns><c>T</c> may be a game specific state representation.</returns>
        T GetState();
    }
}