using System;
using System.Collections.Generic;
using System.Text;

namespace Clients.Game
{
    [Serializable]
    public class Card
    {
        string id;
        public Card()
        {
            id = "";
        }
        public Card(string s)
        {
            id = s;
        }
        public string GetID()
        {
            return id;
        }
    }
    
    [Serializable]
    public class CardSet
    {
        List<Card> RemovedCards;
        public CardSet()
        {
            RemovedCards = new List<Card>();
        }
        public Card Withdraw()
        {
            Card card=new Card();
            char isDuplicate;
            do
            {
                isDuplicate = '0';
                string s;
                Random random = new Random();
                int i = random.Next(1, 14);
                switch (i)
                {
                    case 11:
                        s = "J";
                        break;
                    case 12:
                        s = "Q";
                        break;
                    case 13:
                        s = "K";
                        break;
                    default:
                        s = i.ToString();
                        break;
                }
                i = random.Next(1, 5);
                switch (i)
                {
                    case 1:
                        s = s + "C";
                        break;
                    case 2:
                        s = s + "R";
                        break;
                    case 3:
                        s = s + "H";
                        break;
                    case 4:
                        s = s + "B";
                        break;
                }
               card = new Card(s);
               foreach (Card c in RemovedCards)
               {
                   if (card.GetID() == c.GetID())
                   {
                       isDuplicate = '1';
                   }
               }
            }
            while (isDuplicate=='1');
            RemovedCards.Add(card);
            return card;
        }
    }
}
