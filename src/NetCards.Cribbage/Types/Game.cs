using System;
using System.Linq;
using System.Collections.Generic;
using NetCards.Base;

namespace NetCards.Cribbage
{
    public class Game
    {

        #region Static Members

        private static readonly GameState roundEnd = GameState.Show;

        #endregion

        #region Private Members

        private readonly int handSize;
        private readonly int cribSize;
        private GameState state;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of points required to win the game.
        /// </summary>
        public int VictoryThresh
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the game's current Crib
        /// </summary>
        /// <value>A List of Card objects that make up the Crib.</value>
        public List<Card> Crib
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Deck used by the game
        /// </summary>
        public Deck GameDeck
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Players participating in the game.
        /// </summary>
        /// <value>A list of Player objects.</value>
        public List<Player> Players
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current dealer.
        /// </summary>
        /// <value>The Player object in the Players property that represents
        /// the current dealer.</value>
        public Player Dealer
        {
            get;
            private set;
        }

        /// <summary>
        /// The Winner of the Game.
        /// </summary>
        /// <value>The Player that has won the game. Null until a Player has
        /// enough points to win</value>
        public Player Winner
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for a Cribbage Game class.
        /// </summary>
        /// <param name="numPlayers">The number of players for the game.</param>
        /// <param name="numCards">The number of cards in a player's hand for the game.</param>
        public Game(int numPlayers = 2, int numCards = 4)
        {
            GameDeck = new Deck(DeckType.Traditional);
            handSize = numCards;

            // FUTURE - we should support the different variations of Cribbage.
            // Start with standard, 2-person, 4-card Cribbage for now.
            if (numPlayers != 2 || numCards != 4)
            {
                throw new ArgumentException("Unsupported game configuration");
            }

            cribSize = 4;
            VictoryThresh = 121;
            Crib = new List<Card>();

            // Initialize players and start first one as dealer.
            for (int i = 0; i < numPlayers; i++)
            {
                Players.Add(new Player());
            }
            this.Dealer = Players.ElementAt(0);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Runs a game of Cribbage from beginning to end.
        /// </summary>
        public void Run()
        {
            while (state != GameState.Finished)
            {
                Step();
            }
        }

        /// <summary>
        /// Executes one "phase" (Deal/Play/Show) of Cribbage and advances the
        /// Game state to the appropriate next phase
        /// </summary>
        public void Step()
        {
            PeggingSession session;
            Card cutCard;

            switch (state)
            {
                // Deal hands and discard cards into crib
                case GameState.Deal:
                    {
                        Deal();
                        break;
                    }
                // Cut before Pegging
                case GameState.Cut:
                    GameDeck.Cut();
                    cutCard = GameDeck.Draw();
                    if (cutCard.Name == FaceValue.Jack)
                    {
                        AwardPoints(Dealer, 2);
                    }
                    break;
                // Pegging, or "The Play"
                case GameState.Play:
                    {
                        session = new PeggingSession(this, Players);
                        session.Run();
                        break;
                    }
                // Count points for player hands
                case GameState.Show:
                    {
                        // Future - either extensions or a static evaluator
                        break;
                    }
                default:
                    {
                        // TODO something should catch this.
                        throw new InvalidOperationException("Game Object has an invalid state");
                    }
            }

            AdvanceState();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Deals cards to each player. Each player then selects cards to discard to crib.
        /// </summary>
        private void Deal()
        {
            Player player = NextPlayer(Dealer);
            int discardSize = GetDiscardSize();

            GameDeck.Shuffle();

            while (Dealer.NumCards <= handSize)
            {
                player.Draw(GameDeck);
                player = NextPlayer(player);

                // TODO throw exception if we run out of cards. Need public property from Deck
            }

            // Each player discards a certain number of cards for the crib.
            foreach (Player p in Players)
            {
                Crib.AddRange((p.Discard(discardSize)));
            }
        }

        /// <summary>
        /// Advances the state of the game to the next phase. If a round 
        /// has ended, the deck, the crib, and the player's hands will be 
        /// cleared.
        /// </summary>
        private void AdvanceState()
        {
            if (state == GameState.Finished)
            {
                return;
            }

            // If we've finished a round, reset the cards and start over
            if (state == roundEnd)
            {
                GameDeck = new Deck(DeckType.Traditional);
                Crib.Clear();
                // FUTURE - Loop may not be necessary depending on 
                // implementation of Player and PeggingSession, but be safe 
                // for now.
                foreach (Player p in Players)
                {
                    p.ClearHand(); 
                }
                Dealer = NextPlayer(Dealer); 
            }
            else
            {
                this.state ++;
            }
        }

        /// <summary>
        /// Wrapper to add points to a player's score. Will handle overflow
        /// and will set GameState and Winner appropriately if a player
        /// gets enough points to win. If a Winner is set, no other players
        /// will receive any points.
        /// </summary>
        /// <param name="player">The Player to award points to.</param>
        /// <param name="points">The number of points to award.</param>
        private void AwardPoints(Player player, int points)
        {
            int newScore;

            // Never award points during a round if a winner has already been determined
            if (Winner != null)
            {
                return;
            }

            newScore = player.Points + points;

            // If a player has won, signal that the game should end.
            if (newScore >= VictoryThresh)
            {
                newScore = VictoryThresh;
                state = GameState.Finished;
                Winner = player;
            }

            player.Points = newScore;
        }

        /// <summary>
        /// Returns the next Player in the turn-order.
        /// </summary>
        /// <param name="current">The current Player.</param>
        /// <returns>The Player to act after the current Player.</returns>
        private Player NextPlayer(Player current)
        {
            int nextIndex;
            Player next;

            if (current == Players.Last())
            {
                next = Players.First();
            }
            else
            {
                nextIndex = Players.IndexOf(current) + 1;
                next = Players[nextIndex];
            }

            return next;
        }

        // In different variations, players discard different numbers cards.
        // Simple Dictionary may work, but stubbing a method for now.
        private int GetDiscardSize()
        {
            return 2; // FUTURE - logic for determining the discard size for a game's configuration.
        }

        #endregion
    }
}
