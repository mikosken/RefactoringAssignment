using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNaiveGameEngine;

namespace GameTest
{
    [TestClass]
    public class MooGameStateTest
    {
        private MooGameState _state;

        [TestInitialize]
        public void TestInitialize() {
            // Make sure state is set to a known state.
            _state = new MooGameState("1234567890", 4);
            _state.PlayerName = "Steve";
            _state.Target = "1234";

        }

        [TestMethod]
        public void TestInitialization()
        {
            Assert.AreEqual("1234567890", _state.AllowedCharacters);
            Assert.AreEqual("Steve", _state.PlayerName);
            Assert.AreEqual("1234", _state.Target);
        }

        [TestMethod]
        public void TestGuessing()
        {
            // All wrong guess.
            _state.Guess("0987");
            Assert.AreEqual(",", _state.ToString());

            // Two characters correct, wrong places.
            _state.Guess("0912");
            Assert.AreEqual(",CC", _state.ToString());
            Assert.AreEqual(false, _state.Success);
            // Two characters correct, one in correct place.
            _state.Guess("1982");
            Assert.AreEqual("B,C", _state.ToString());
            Assert.AreEqual(false, _state.Success);
            // All characters correct, in correct place.
            _state.Guess("1234");
            Assert.AreEqual("BBBB,", _state.ToString());
            Assert.AreEqual(true, _state.Success);
        }

        [TestMethod]
        public void TestGenerateTarget()
        {
            Assert.AreEqual(4, _state.Target.Count());
            _state.GenerateTarget();
            Assert.AreEqual(4, _state.Target.Count());

            // Very unlikely we get the same target twice in a row.
            var initialTarget = _state.Target;
            _state.GenerateTarget();
            Assert.AreNotEqual(initialTarget, _state.Target);
        }

        [TestMethod]
        public void TestTryCountOnFirstSuccess()
        {
            Assert.AreEqual(false, _state.Success);
            Assert.AreEqual(0, _state.TryCountOnFirstSuccess);

            _state.Guess("");
            Assert.AreEqual(false, _state.Success);
            Assert.AreEqual(0, _state.TryCountOnFirstSuccess);

            _state.Guess("0987");
            Assert.AreEqual(false, _state.Success);
            Assert.AreEqual(0, _state.TryCountOnFirstSuccess);

            _state.Guess("1234");
            Assert.AreEqual(true, _state.Success);
            Assert.AreEqual(3, _state.TryCountOnFirstSuccess);

            _state.Guess("5555");
            Assert.AreEqual(true, _state.Success);
            Assert.AreEqual(3, _state.TryCountOnFirstSuccess);
        }
    }
}