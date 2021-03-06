using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCards.Base;

namespace NetCards.UnitTests
{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void DeckShouldCreateTest() {
            Deck testDeck = new Deck(DeckType.Traditional);

            Assert.IsNotNull(testDeck);
        }

        [TestMethod]
        public void DeckDrawTest()
        {
            Deck testDeck = new Deck(DeckType.Traditional);

            Card drawnCard = testDeck.Draw();

            Assert.IsNotNull(drawnCard);
            Assert.IsTrue(drawnCard.Values.Count > 0);
        }

        [TestMethod]
        public void DeckCutTest() {
            Deck testDeck = new Deck();

            testDeck.Cut();

            Card drawnCard = testDeck.Draw();

            Assert.IsNotNull(drawnCard);
            Assert.IsTrue(drawnCard.Values.Count > 0);
        }

        [TestMethod]
        public void DeckShuffleTest() {
            Deck testDeck = new Deck();

            testDeck.Shuffle();

            Card drawnCard = testDeck.Draw();

            Assert.IsNotNull(drawnCard);
            Assert.IsTrue(drawnCard.Values.Count > 0);
        }
    }
}
