using System;
using System.Collections.Generic;

namespace Clients.Game
{
    [Serializable]
    public class Player
    {
        char type;//If 0: ,1:
        public List<Card> cards;
        public int numberOfCards;
        public Matrix matrix;
        int status;
        public Player()
        {
            cards = new List<Card>();
            numberOfCards = 0;
            matrix = new Matrix();
        }
        public void SetType(char c)
        {
            type = c;
        }
        public int GetNumberOfCards()
        {
            return numberOfCards;
        }
        public char GetT()
        {
            return type;
        }
        public List<Card> GetCards()
        {
            return cards;
        }
        public Card WithdrawCard(CardSet cardSet)
        {
            Card card = cardSet.Withdraw();
            cards.Add(card);
            numberOfCards++;
            int i;
            string s=card.GetID().Substring(0,card.GetID().Length==2?1:2);
            switch(s)
            {
                case "J":
                    i=10;
                    break;
                case "Q":
                    i = 10;
                    break;
                case "K":
                    i = 10;
                    break;
                default:
                    i=int.Parse(s);
                    break;
            }
            matrix.Add(i);
            status=matrix.GetStatus();
            return card;
        }
        public int GetStatus()
        {
            return status;
        }
        public int GetMark()
        {
            matrix.Reduce();
            return matrix.GetMaxSum();
        }
    }

    [Serializable]
    public class GameStatue
    {
        public bool gameEnd;
        public Player dealerHand;
    }

}
