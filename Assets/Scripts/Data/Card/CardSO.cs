using Sirenix.OdinInspector;
using TSoft.InGame;
using TSoft.InGame.CardSystem.CE;
using UnityEngine;

namespace TSoft.Data.Card
{
    [CreateAssetMenu(fileName = "Card", menuName = "Data/Card", order = 0)]
    public class CardSO : ScriptableObject
    {
        public string title;
        public string description;
        public Sprite image;
        public int number;
        public CardType type;

        [ShowIf("type", CardType.Joker), SerializeReference]
        public CustomEffect effect;
        
        [ShowIf("type", CardType.Joker)]
        public CustomEffectPolicy policy;
    }
}
