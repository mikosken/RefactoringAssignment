using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNaiveGameEngine;

namespace GameTest.Tests
{
    public class PlayerScoreTest
    {
        List<PlayerScore> scores;

        [TestInitialize]
        public void TestInitialize() {
            scores = new List<PlayerScore>();
            scores.Add(new PlayerScore("Steve", 8));
            scores.Add(new PlayerScore("Steve", 10));
            scores.Add(new PlayerScore("John", 5));
            scores.Add(new PlayerScore("Alex", 10));
        }

        [TestMethod]
        public void TestToToplist() {
            var toplist = scores.ToToplist();

            var steve = toplist.First(i => i.Name == "Steve");
            var s = steve.ToString("{NAME,-9}{GAMECOUNT,5:D}{AVERAGE,9:F2}");
            Assert.AreEqual("Steve        2     9,00", s);

            var alex = toplist.First(i => i.Name == "Alex");
            var a = alex.ToString("{NAME,-9}{GAMECOUNT,5:D}{AVERAGE,9:F2}");
            Assert.AreEqual("Alex         1      10,00", s);
        }
    }
}