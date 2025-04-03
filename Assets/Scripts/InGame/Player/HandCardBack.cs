using HF.Data.Card;
using UnityEngine;

namespace HF.InGame.Player
{
    public class HandCardBack : MonoBehaviour
    {
        public SpriteRenderer card_sprite;
       
        public void SetCardback(CardBackData cb)
        {
            if (cb != null && cb.cardback != null)
                card_sprite.sprite = cb.cardback;
        }

    }
}
