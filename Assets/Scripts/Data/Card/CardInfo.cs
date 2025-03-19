using Sirenix.OdinInspector;
using TSoft.InGame;
using TSoft.InGame.CardSystem.CE;
using TSoft.InGame.GamePlaySystem;
using TSoft.Item;
using UnityEngine;

namespace TSoft.Data.Card
{
    [CreateAssetMenu(fileName = "Card", menuName = "Data/Card", order = 0)]
    public class CardInfo : ItemInfo
    {
        public int number;
        public CardType type;
        
        [ShowIf("type", CardType.Joker)]
        public GameplayEffectSO.GameplayEffect instantEffect;
    }
}
