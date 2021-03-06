using System;
using System.Collections.Generic;
using System.Text;
using NetCards.Base;

namespace NetCards.Cribbage
{
    // TODO should we make this an interface or base class?
    public class Player
    {
        private List<Card> Hand;

        public int Points
        {
            get;
            set;
        }

        public int NumCards
        {
            get
            {
                return Hand.Count;
            }
        }

        // TODO maybe rename this or the deck method
        public void Draw(Deck deck)
        {
            Hand.Add(deck.Draw());
        }

        public void ClearHand()
        {
            Hand.Clear();
        }

        public List<Card> Discard(int numToDiscard)
        {
            List<Card> selection = new List<Card>();

            // just discard first 2 cards for now
            for (int i = 0; i < numToDiscard; i++)
            {
                selection.Add(Hand[0]);
                Hand.RemoveAt(0);
            }

            return selection;
        }
    }
}
