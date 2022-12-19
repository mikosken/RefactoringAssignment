namespace MyNaiveGameEngine
{
    public class ConsoleGameIO : IGameIO
    {
        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string? value)
        {
            Console.WriteLine(value);
        }

        public T? ListSelect<T>(List<T> items) {
            while(true) {
                WriteLine("");

                int index = 0;
                foreach(var item in items) {
                    index++; // Note, first in printed list is 1.
                    WriteLine(String.Format("{0, -4} {1, -20}", index, item.ToString()));
                }

                WriteLine("Enter number to select or q to cancel:");
                var answer = ReadLine();
                if (answer != null && answer != "") {
                    if (answer.Substring(0, 1).ToLower() == "q")
                        return default(T);

                    if (int.TryParse(answer, out int selection) &&
                        selection > 0 &&
                        selection <= items.Count() ) {
                        return items[selection - 1];
                    }
                }
            }
        }
    }
}