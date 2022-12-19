using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyNaiveGameEngine
{
    public class PlayerData
    {
        public string Name { get; private set; }
        public int GameCount { get; private set; }
        public int TotalGuessCount { get; private set; }

		public PlayerData(string name, int gameCount, int totalGuessCount)
		{
			this.Name = name;
            this.GameCount = gameCount;
            this.TotalGuessCount = totalGuessCount;
		}

		public PlayerData(string name, int guesses)
		{
			this.Name = name;
			this.GameCount = 1;
			this.TotalGuessCount = guesses;
		}

        /// <summary>
        /// Formats and returns this PlayerData as a string.
        /// </summary>
        /// <param name="format">The string template.
        /// In addition to regular formatting tokens you can also use:
        /// NAME : Is replaced by the Name property.
        /// GAMECOUNT : Is replaced by the GameCount property
        /// GUESSES : Is replaced by the TotalGuessCount property.
        /// AVERAGE : Is replaced by the value from Average().
        /// Example: "{NAME,-9}{GAMECOUNT,5:D}{AVERAGE,9:F2}"
        /// </param>
        /// <returns></returns>
        public string ToString(string format)
        {
            format = format.Replace("NAME", "0");
            format = format.Replace("GAMECOUNT", "1");
            format = format.Replace("GUESSES", "2");
            format = format.Replace("AVERAGE", "3");
            return String.Format(format,
                this.Name,this.GameCount, this.TotalGuessCount, this.Average());
        }

        public override string ToString() {
            return $"{this.Name}, GameCount: {this.GameCount}, Avg: {this.Average}";
        }

        public void Update(int guesses)
		{
			this.TotalGuessCount += guesses;
			GameCount++;
		}

		public double Average()
		{
			return (double)TotalGuessCount / GameCount;
		}

		
	    public override bool Equals(Object p)
		{
			return Name.Equals(((PlayerData)p).Name);
		}

		
	    public override int GetHashCode()
        {
			return Name.GetHashCode();
		}
    }
}