namespace MyNaiveGameEngine
{
    public class MastermindGameConfiguration
    {
        public string ScoreFile { get; set; } = "mastermind_result.txt";
        public string AllowedCharacters { get; set; } = "123456";
        public int NumberOfCharactersInTarget { get; set; } = 4;
        public bool PracticeMode { get; set; } = false;
    }
}