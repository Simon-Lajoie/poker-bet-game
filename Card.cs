using System;
namespace PokerBetGame
{
    public class Card
    {
        public enum Suit { Hearts, Diamonds, Clubs, Spades };
        public enum FaceValue { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };

        private string[] faceValuesString = new string[] { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };

        private FaceValue _faceValue;
        private Suit _suit;

        // constructor
        public Card()
        {
            this._suit = Suit.Spades;
            this._faceValue = FaceValue.Ace;
        }

        public Card(Suit theSuit, FaceValue theFaceValue)
        {
            this._suit = theSuit;
            this._faceValue = theFaceValue;
        }

        public override string ToString()
        {
            return faceValuesString[(int)this._faceValue] + " of " + this._suit;
        }

        public Suit getSuit()
        {
            return this._suit;
        }

        public FaceValue getFaceValue()
        {
            return this._faceValue;
        }

    }
}
