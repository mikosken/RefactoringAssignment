using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNaiveGameEngine;

namespace GameTest.Tests
{
    public class PlayerDataTest
    {
        PlayerData playerData;

        [TestInitialize]
        public void TestInitialize() {
            playerData = new PlayerData("John", 7, 14);
        }

        [TestMethod]
        public void TestToString() {
            var s = playerData.ToString("{NAME,-9}{GAMECOUNT,5:D}{AVERAGE,9:F2}");
            Assert.AreEqual("John         7     2,00", s);
        }
    }
}