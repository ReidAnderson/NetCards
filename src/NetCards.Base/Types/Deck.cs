using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCards.Base
{
    /// <summary>A deck manages the list of cards that can be drawn from</summary>
    public class Deck : ICardDeck
    {
        private List<Card> deck = new List<Card>();
        private Random rnd = new Random();

        public Deck(DeckType deckType = DeckType.Traditional, int? seed = null)
        {
            if (seed.HasValue)
            {
                rnd = new Random(seed.Value);
            }

            switch (deckType)
            {
                case DeckType.Traditional:
                    deck = TraditionalCards();
                    break;
                default:
                    throw new ArgumentException("Unsupported deck type");
            }
        }

        /// <summary> Return how many cards remain in the deck </summary>
        /// <returns> The number of cards left</returns>
        public int CardsLeft()
        {
            return deck.Count;
        }

        /// <summary> Draw the next card in the deck, removing it from the deck </summary>
        /// <returns> The card removed from the deck </returns>
        public Card Draw()
        {
            if (deck.Count == 0)
            {
                // Attempting to draw when there aren't any cards left is valid in some games
                // As a result we just return null instead of throwing an exception
                return null;
            }

            Card result = deck[0];
            deck.RemoveAt(0);

            return result;
        }

        /// <summary> Randomize the order of the cards in the deck </summary>
        public void Shuffle()
        {
            deck = deck.OrderBy((item) => rnd.Next()).ToList();
        }

        /// <summary> Cut the deck. Take the cards above a certain point and place them on the bottom of the deck </summary>
        public void Cut()
        {
            List<Card> cutDeck = new List<Card>();
            int cutPosition = rnd.Next(0, deck.Count);

            cutDeck.AddRange(deck.GetRange(cutPosition, deck.Count - cutPosition));
            cutDeck.AddRange(deck.GetRange(0, cutPosition));

            deck = cutDeck;
        }

        private List<Card> TraditionalCards()
        {
            List<Card> deck = new List<Card>();
            foreach (FaceValue faceValue in Enum.GetValues(typeof(FaceValue)))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    deck.Add(new Card(faceValue, suit, DeckType.Traditional));
                }
            }

            return deck;
        }
    }
}
