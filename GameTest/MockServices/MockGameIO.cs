using System.Collections.Generic;
using MyNaiveGameEngine;

namespace GameTest.MockServices
{
    public class MockGameIO : IGameIO
    {
        public string ReturnString { get; set; }
        public List<string> WrittenLines { get; set; } = new List<string>();

        public T? ListSelect<T>(List<T> items)
        {
            throw new System.NotImplementedException();
        }

        public string? ReadLine()
        {
            return ReturnString;
        }

        public void WriteLine(string value)
        {
            WrittenLines.Add(value);
            return;
        }
    }
}