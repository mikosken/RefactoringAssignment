namespace MyNaiveGameEngine
{
    public class BullsAndCowsGameConfiguration
    {
        public string ScoreFile { get; set; } = "result.txt";
        public string AllowedCharacters { get; set; } = "1234567890";
        public int NumberOfCharactersInTarget { get; set; } = 4;
        public bool PracticeMode { get; set; } = false;
    }
}