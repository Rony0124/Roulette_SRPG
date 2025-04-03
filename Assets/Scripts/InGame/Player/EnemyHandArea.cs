using System.Collections.Generic;
using HF.Data.Card;
using HF.GamePlay;
using TSoft.Data.Registry;
using UnityEngine;

namespace HF.InGame.Player
{
    public class EnemyHandArea : HandArea
    {
        private List<HandCardBack> cards = new List<HandCardBack>();
        
        void Update()
        {
            var dir = GameContext.Instance.CurrentDirector as InGameDirector;
            if (dir == null)
                return;
            
            if(dir.Combat.GameData == null)
                return;

            Game data = dir.Combat.GameData;
            GamePlay.Player player = data.GetOpponentPlayer(CombatController.OwnerPlayerId);

            if (cards.Count < player.cards_hand.Count)
            {
                GameObject new_card = Instantiate(card_prefab, card_area);
                HandCardBack hand_card = new_card.GetComponent<HandCardBack>();
                var cbdata = DataRegistry.Instance.CardBackDataRegistry.Get(player.cardback);
                hand_card.SetCardback(cbdata);
                Transform card_rect = new_card.transform;
                card_rect.localPosition = new Vector2(0f, 1f);
                cards.Add(hand_card);
            }

            if (cards.Count > player.cards_hand.Count)
            {
                HandCardBack card = cards[cards.Count - 1];
                cards.RemoveAt(cards.Count - 1);
                Destroy(card.gameObject);
            }

            int nb_cards = Mathf.Min(cards.Count, player.cards_hand.Count);

            for (int i = 0; i < nb_cards; i++)
            {
                HandCardBack card = cards[i];
                Transform cTr = card.transform;
                float half = nb_cards / 2f;
                Vector3 tpos = new Vector3((i - half) * card_spacing, (i - half) * (i - half) * card_offset_y);
                float tangle = (i - half) * card_angle;
                cTr.localPosition = Vector3.Lerp(cTr.localPosition, tpos, 4f * Time.deltaTime);
                card.transform.localRotation = Quaternion.Slerp(card.transform.localRotation, Quaternion.Euler(0f, 0f, tangle), 4f * Time.deltaTime);
            }

        }
    }
}
