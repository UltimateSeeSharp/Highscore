using Highscore.Code.AccessLayer;

namespace Highscores.Tests
{
    public class Tests
    {
        private readonly HighscoreDbAccessLayer _highscoreAccessLayer = new();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}