using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCards.Base
{
    public class Deck : ICardDeck
    {
        private List<Card> deck = new List<Card>();
        private Random rnd = null;

        public Deck(DeckType deckType = DeckType.Traditional, int? seed = null)
        {
            if (seed.HasValue) {
                rnd = new Random(seed.Value);
            } else {
                rnd = new Random();
            }
            
            switch (deckType)
            {
                case DeckType.Traditional:
                    deck = Constants.TraditionalDeck();
                    break;
                default:
                    throw new Exception("Unsupported deck type");
            }
        }

        public Card Draw()
        {
            if (deck.Count == 0) {
                // Attempting to draw when there aren't any cards left is valid in some games
                // As a result we just return null instead of throwing an exception
                return null;
            }

            Card result = deck[0];
            deck.RemoveAt(0);

            return result;
        }

        public void Shuffle()
        {
            deck = deck.OrderBy((item) => rnd.Next()).ToList();
        }

        public void Cut()
        {
            List<Card> cutDeck = new List<Card>();
            Random rnd = new Random();
            int cutPosition = rnd.Next(0, deck.Count);

            cutDeck.AddRange(deck.GetRange(cutPosition, deck.Count-cutPosition));
            cutDeck.AddRange(deck.GetRange(0, cutPosition));

            deck = cutDeck;
        }
    }
}
