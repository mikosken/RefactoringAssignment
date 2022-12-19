using System.Collections.Generic;
using System.Linq;
using GameTest.MockServices;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNaiveGameEngine;

namespace GameTest
{
    [TestClass]
    public class MooGameTest
    {
        private MockGameIO _gameIO;
        private MockScoreStore _scoreStore;
        private IConfiguration _config;
        private MooGame _mooGame;

        [TestInitialize]
        public void TestInitialize() {
            _gameIO = new MockGameIO();
            _scoreStore = new MockScoreStore();
            // Create mock settings.
            var inMemorySettings = new Dictionary<string, string> {
                {"MooGame:ScoreFile", "FakeFile.txt"},
                {"MooGame:AllowedCharacters", "12345"},
                {"MooGame:NumberOfCharactersInTarget", "3"},
                {"MooGame:PracticeMode", "false"}
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _mooGame = new MooGame(_gameIO, _scoreStore, _config);
        }

        [TestMethod]
        public void TestInitialGameState() {
            // Mock settings change initial state from defaults to these.
            Assert.AreEqual("12345", ((MooGameState)_mooGame.GetState()).AllowedCharacters);
            Assert.AreEqual(3, ((MooGameState)_mooGame.GetState()).NumberOfCharactersInTarget);
        }

        [TestMethod]
        public void TestFirstStep() {
            // Set what ReadLine() will return.
            _gameIO.ReturnString = "John";
            _mooGame.FirstStep();
            Assert.AreEqual("John", ((MooGameState)_mooGame.GetState()).PlayerName);

            // Target only contains numbers, but when asking for input it
            // currently returns "John". So no correct numbers.
            Assert.AreEqual(",\n", _gameIO.WrittenLines.Last());
        }

        [TestMethod]
        public void TestStep() {
            Assert.AreEqual(false, ((MooGameState)_mooGame.GetState()).Success);

            // Set what ReadLine() will return to two correct numbers.
            _gameIO.ReturnString = ((MooGameState)_mooGame.GetState()).Target.Substring(0,2);
            _mooGame.Step();
            Assert.AreEqual(false, ((MooGameState)_mooGame.GetState()).Success);
            Assert.AreEqual("BB,\n", _gameIO.WrittenLines.Last());

            // Set what ReadLine() will return to correct Target.
            _gameIO.ReturnString = ((MooGameState)_mooGame.GetState()).Target;
            _mooGame.Step();
            Assert.AreEqual(true, ((MooGameState)_mooGame.GetState()).Success);
        }

        [TestMethod]
        public void TestLastStep() {
            // Set what ReadLine() will return to two correct numbers.
            _gameIO.ReturnString = "Very wrong guess";
            _mooGame.Step();
            // Set what ReadLine() will return to correct Target.
            _gameIO.ReturnString = ((MooGameState)_mooGame.GetState()).Target;
            _mooGame.Step();

            // Finally test LastStep()
            _mooGame.LastStep();
            Assert.AreEqual("Correct, it took 2 guesses", _gameIO.WrittenLines.Last());

        }
    }
}