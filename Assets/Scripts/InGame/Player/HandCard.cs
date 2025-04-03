using System;
using System.Collections.Generic;
using HF.GamePlay;
using UnityEngine;

namespace HF.InGame.Player
{
    public class HandCard : MonoBehaviour
    {
        public SpriteRenderer card_sprite;
        
        public float move_speed = 10f;
        
        [HideInInspector]
        public Vector2 deck_position;
        [HideInInspector]
        public float deck_angle;

        private Guid uid;
        
        private Transform card_transform;
        private Vector3 current_rotate;
        
        private bool destroyed = false;
        
        private static List<HandCard> card_list = new List<HandCard>();

        public Guid Uid => uid;

        private void Awake()
        {
            card_list.Add(this);
            card_transform = transform.GetComponent<Transform>();
        }
        
        private void OnDestroy()
        {
            card_list.Remove(this);
        }
        
        void Update()
        {
            var dir = GameContext.Instance.CurrentDirector as InGameDirector;
            if (dir == null)
                return;
            
            if(dir.Combat.GameData == null)
                return;
            
            Vector2 target_position = deck_position;
            current_rotate = new Vector3(0f, 0f, deck_angle);
            
            card_transform.localPosition = Vector2.Lerp(card_transform.localPosition, target_position, Time.deltaTime * move_speed);
            card_transform.localRotation = Quaternion.Slerp(card_transform.localRotation, Quaternion.Euler(current_rotate), Time.deltaTime * move_speed);
        }
        
        public void SetCard(Card card)
        {
            uid = card.uid;
            card_sprite.sprite = card.Data.artInGame;
            // card_ui.SetCard(card);
        }
        
        public void Kill()
        {
            if (!destroyed)
            {
                destroyed = true;
                Destroy(gameObject);
            }
        }
        
        public Guid GetCardUID()
        {
            return uid;
        }

        public void SetOrder(int order)
        {
            card_sprite.sortingOrder = order;
        }
        
        public static HandCard Get(Guid uid)
        {
            foreach (HandCard card in card_list)
            {
                if (card && card.GetCardUID() == uid)
                    return card;
            }
            return null;
        }
    }
}
