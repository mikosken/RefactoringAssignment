namespace MyNaiveGameEngine
{
    public class ConsoleIO : IConsoleIO
    {
        string? IConsoleIO.ReadLine()
        {
            return Console.ReadLine();
        }

        void IConsoleIO.WriteLine(string? value)
        {
            Console.WriteLine(value);
        }
    }
}