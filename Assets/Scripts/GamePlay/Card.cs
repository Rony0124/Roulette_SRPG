using System;
using System.Collections.Generic;
using HF.Data.Card;

namespace HF.GamePlay
{
    public class Card
    {
        public Guid uid;
        public int playerId;

        public CardData Data;

        public Card(Guid guid, int playerId)
        {
            uid = guid; 
            this.playerId = playerId;
        }
        
        public void SetCard(CardData icard)
        {
            Data = icard;
            uid = icard.Id.Value;
        }
        
        public static Card Create(CardData icard, Player player)
        {
            Card card = new Card(icard.Id.Value, player.player_id);
            card.SetCard(icard);
            player.cards_all[icard.Id.Value] = card;
            return card;
        }
        
        public static Card CloneNew(Card source)
        {
            Card card = new Card(source.uid, source.playerId);
            Clone(source, card);
            return card;
        }
        
        //Clone all card variables into another var, used mostly by the AI when building a prediction tree
        public static void Clone(Card source, Card dest)
        {
            dest.uid = source.uid;
            dest.playerId = source.playerId;
            dest.Data = source.Data;
        }
        
        public static void CloneDict(Dictionary<Guid, Card> source, Dictionary<Guid, Card> dest)
        {
            foreach (KeyValuePair<Guid, Card> pair in source)
            {
                bool valid = dest.TryGetValue(pair.Key, out Card val);
                if (valid)
                    Clone(pair.Value, val);
                else
                    dest[pair.Key] = CloneNew(pair.Value);
            }
        }

        //Clone list by keeping references from ref_dict
        public static void CloneListRef(Dictionary<Guid, Card> ref_dict, List<Card> source, List<Card> dest)
        {
            for (int i = 0; i < source.Count; i++)
            {
                Card scard = source[i];
                bool valid = ref_dict.TryGetValue(scard.uid, out Card rcard);
                if (valid)
                {
                    if (i < dest.Count)
                        dest[i] = rcard;
                    else
                        dest.Add(rcard);
                }
            }

            if(dest.Count > source.Count)
                dest.RemoveRange(source.Count, dest.Count - source.Count);
        }
    }
}
