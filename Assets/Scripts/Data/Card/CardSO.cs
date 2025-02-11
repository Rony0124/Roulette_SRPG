using Sirenix.OdinInspector;
using TSoft.InGame;
using TSoft.InGame.CardSystem.CE;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Data.Card
{
    [CreateAssetMenu(fileName = "Card", menuName = "Data/Card", order = 0)]
    public class CardSO : ItemSO
    {
        public int number;
        public CardType type;
        
        [ShowIf("type", CardType.Joker), SerializeReference]
        public CustomEffect customEffect;
        
        [ShowIf("type", CardType.Joker)]
        public GameplayEffectSO effect;
    }
}
