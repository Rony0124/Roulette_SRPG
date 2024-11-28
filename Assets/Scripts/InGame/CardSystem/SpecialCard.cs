using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public class SpecialCard : MonoBehaviour
    {
        public SpecialCardData cardData;
        
        public void SetData(SpecialCardData card)
        {
            cardData = card;
        }
    }
}
