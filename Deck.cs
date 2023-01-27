using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerBetGame
{
    public class Deck
    {
        // =====================================================================================         
        // global variables         
        // ===================================================================================== 
        public const int NUM_OF_CARDS_IN_DECK = 52;
        private const int TIMES_TO_SHUFFLE = 3;

        private Card[] deck = new Card[NUM_OF_CARDS_IN_DECK];
        private int currentCard = 0;

        //contructor
        public Deck()
        {
            currentCard = 0;

            foreach (Card.Suit s in Enum.GetValues(typeof(Card.Suit)))
            {
                foreach (Card.FaceValue f in Enum.GetValues(typeof(Card.FaceValue)))
                {
                    deck[currentCard++] = new Card(s, f);
                }
            }
			
			currentCard = 0;
        }


        // =====================================================================================             
        // Shuffle the deck and generate random cards                
        // ===================================================================================== 
        public void shuffle()
        {
            currentCard = 0;

            Random random = new Random();
            Card tempCard;
      

            for (int shuffle = 0; shuffle < TIMES_TO_SHUFFLE; shuffle++)
            {
                for (int loop = 0; loop < NUM_OF_CARDS_IN_DECK; loop++)
                {
                    int rand = random.Next(NUM_OF_CARDS_IN_DECK);

                    tempCard = deck[rand];
                    deck[rand] = deck[loop];
                    deck[loop] = tempCard;
                }
            }
        }


        // Pre-Condtion - the deck cannot be empty.
        // isEmpty is verified in the application before calling dealACard()
        public Card dealACard()
        {
            return this.deck[currentCard++];
        }

        public bool isEmpty()
        {
            return currentCard >= NUM_OF_CARDS_IN_DECK;
        }

    }
}



































