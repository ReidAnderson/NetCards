namespace NetCards.Base
{
    public interface ICardDeck
    {
        Card Draw();
        void Shuffle();
        void Cut();
    }
}
