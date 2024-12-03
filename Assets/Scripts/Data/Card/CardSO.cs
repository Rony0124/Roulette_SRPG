using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    [CreateAssetMenu(fileName = "Card", menuName = "Data/Card", order = 0)]
    public class CardSO : ScriptableObject
    {
        public CardData Data;
    }
}
