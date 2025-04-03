using System;
using System.Collections.Generic;

namespace HF.GamePlay
{
    [Serializable]
    public class Player
    {
        public int player_id;
        public Guid cardback;
        
        public bool is_ai;
        public bool ready = false;
        
        public Dictionary<Guid, Card> cards_all = new Dictionary<Guid, Card>();
        
        public List<Card> cards_deck = new List<Card>();
        public List<Card> cards_hand = new List<Card>();  
        public List<Card> cards_discard = new List<Card>();

        public Player(int id)
        {
            player_id = id;
        }
        
        public Card GetHandCard(Guid uid)
        {
            foreach (Card card in cards_hand)
            {
                if (card.uid == uid)
                    return card;
            }
            return null;
        }
    }
}
