using System.Collections.Generic;

namespace NetCards.Base
{
    public class Card
    {
        public Card(FaceValue faceValue, Suit suit, DeckType deckType) {
            this.Name = faceValue;
            this.Suit = suit;
            this.SortValues = new List<int>() { (int) faceValue };
            this.Values = new List<int>() { (int) faceValue };

            // Specific overrides for different deck types
            if (deckType == DeckType.Traditional) {
                if (faceValue == FaceValue.Jack || faceValue == FaceValue.Queen || faceValue == FaceValue.King) {
                    this.Values = new List<int>() { 10 };
                }
            }
        }

        public FaceValue Name { get; set; }
        public List<int> Values { get; set; }
        public List<int> SortValues { get; set; }
        public Suit Suit { get; set; }
    }
}