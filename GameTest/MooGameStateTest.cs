using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNaiveGameEngine;

namespace GameTest
{
    [TestClass]
    public class MooGameStateTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var state = new MooGameState();

            Assert.AreEqual("1234567890", state.AllowedCharacters);
        }
    }
}