namespace MyNaiveGameEngine
{
    public interface IGame
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
        void LoadState(IGameState state);

        /// <summary>
        /// Run the game for one game step.
        /// Only called once.
        /// </summary>
        void FirstStep();

        /// <summary>
        /// Run the game for one game step.
        /// </summary>
        void Step();

        /// <summary>
        /// Run the game for one game step.
        /// Only called once.
        /// </summary>
        void LastStep();

        /// <summary>
        /// Returns the current state of the game.
        /// </summary>
        /// <returns>IGameState is a marker interface. May be a game specific state implementation.</returns>
        IGameState GetState();

        /// <summary>
        /// Starts the game loop.
        /// </summary>
        void Run();
    }
}