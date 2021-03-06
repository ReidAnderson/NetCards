using System;
using System.Collections.Generic;

namespace NetCards.Base
{
    public static class Constants
    {
        public static List<Card> TraditionalDeck() {
            List<Card> deck = new List<Card>();
            foreach(FaceValue faceValue in (FaceValue[])Enum.GetValues(typeof(FaceValue))) {
                foreach(Suit suit in (Suit[])Enum.GetValues(typeof(Suit))) {
                    deck.Add(new Card(faceValue, suit, DeckType.Traditional));
                }
            }

            return deck;
        }
    }
}