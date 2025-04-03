using System;
using System.Collections.Generic;
using HF.GamePlay;
using UnityEngine;

namespace HF.InGame.Player
{
    public class PlayerHandArea : HandArea
    {
        private List<HandCard> cards = new List<HandCard>();
        
        private Guid last_destroyed;
        private float last_destroyed_timer = 0f;
        
        void Update()
        {
            var dir = GameContext.Instance.CurrentDirector as InGameDirector;
            if (dir == null)
                return;
            
            if(dir.Combat.GameData == null)
                return;
            
            int player_id = CombatController.OwnerPlayerId;
            Game data = dir.Combat.GameData;
            GamePlay.Player player = data.GetPlayer(player_id);

            last_destroyed_timer += Time.deltaTime;

            //Add new cards
            foreach (Card card in player.cards_hand)
            {
                if (!HasCard(card.uid))
                    SpawnNewCard(card);
            }

            //Remove destroyed cards
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                HandCard card = cards[i];
                if (card == null || player.GetHandCard(card.Uid) == null)
                {
                    cards.RemoveAt(i);
                    if (card != null)
                    {
                        card.Kill();
                    }
                }
                
                cards[i].SetOrder(i);
            }
            
            int mid = cards.Count / 2;

            if (cards.Count % 2 == 1) // 홀수 개 카드
            {
                cards[mid].deck_position = new Vector2(0, hand_offset_y);
                cards[mid].deck_angle = 0;

                for (int i = 1; i <= mid; i++)
                {
                    float xOffset = i * card_spacing;
                    float yOffset = - (i * i) * card_offset_y + hand_offset_y; // 포물선 형태 적용
                    float cardAngle = i * card_angle;

                    // 오른쪽 카드 배치
                    if (mid + i < cards.Count)
                    {
                        cards[mid + i].deck_position = new Vector2(xOffset, yOffset);
                        cards[mid + i].deck_angle = -cardAngle;
                    }

                    // 왼쪽 카드 배치
                    if (mid - i >= 0)
                    {
                        cards[mid - i].deck_position = new Vector2(-xOffset, yOffset);
                        cards[mid - i].deck_angle = cardAngle;
                    }
                }
            }
            else // 짝수 개 카드
            {
                float startX = -(0.5f * card_spacing); // 좌측 첫 카드 위치 조정
                float startY = -(0.5f * card_offset_y) + hand_offset_y;
                float startRot = 0.5f * card_angle;

                for (int i = 1; i <= mid; i++)
                {
                    float xOffset = startX + i * card_spacing;
                    float yOffset = - (i * i) * card_offset_y + startY; // 부드러운 포물선
                    float cardAngle = i * card_angle + startRot; // 균형 잡힌 회전

                    // 왼쪽 카드 배치
                    cards[mid-i].deck_position = new Vector2(xOffset, yOffset);
                    cards[mid-i].deck_angle = -cardAngle;

                    // 오른쪽 카드 배치
                    
                    cards[mid+i -1].deck_position = new Vector2(-xOffset, yOffset);
                    cards[mid+i -1].deck_angle = cardAngle;
                }
            }
        }
        
        public void SpawnNewCard(Card card)
        {
            GameObject card_obj = Instantiate(card_prefab, card_area.transform);
            card_obj.GetComponent<HandCard>().SetCard(card);
            card_obj.GetComponent<Transform>().position = new Vector2(0f, -1f);
            cards.Add(card_obj.GetComponent<HandCard>());
        }
        
        public bool HasCard(Guid card_uid)
        {
            HandCard card = HandCard.Get(card_uid);
            bool just_destroyed = card_uid == last_destroyed && last_destroyed_timer < 0.7f;
            return card != null || just_destroyed;
        }
    }
}
