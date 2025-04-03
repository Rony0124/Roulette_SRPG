using System;
using HF.Data.Card;

namespace HF.GamePlay
{
    public class Card
    {
        public Guid uid;
        public int playerId;
        
        private CardData data = null;

        public CardData Data => data;

        public Card(Guid guid, int playerId)
        {
            uid = guid; 
            this.playerId = playerId;
        }
        
        public void SetCard(CardData icard)
        {
            data = icard;
            uid = icard.Id.Value;
        }
        
        public static Card Create(CardData icard, Player player)
        {
            Card card = new Card(icard.Id.Value, player.player_id);
            card.SetCard(icard);
            player.cards_all[icard.Id.Value] = card;
            return card;
        }
    }
}
