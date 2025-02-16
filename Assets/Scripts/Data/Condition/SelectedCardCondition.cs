using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TSoft.InGame;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Data.Condition
{
    [CreateAssetMenu(fileName = "Condition", menuName = "Create Condition/SelectedCardCondition", order = 1)]
    public class SelectedCardCondition : ConditionSO
    {
        public enum OddEven
        {
            Odd,
            Even
        }
        
        public enum CardConditionType
        {
            OddEven,
            CardPattern,
            CardType,
            NumberCombination
        }
        
        public CardConditionType conditionType;
            
        [ShowIf("conditionType", CardConditionType.CardPattern)]
        public CardPatternType cardPatternType;

        [ShowIf("conditionType", CardConditionType.OddEven)]
        public OddEven oddEven;
        
        [ShowIf("conditionType", CardConditionType.NumberCombination)]
        public int[] numberCombination;
        
        [ShowIf("conditionType", CardConditionType.CardType)]
        public CardType cardType;

        public override bool Interpret(ConditionApplier applier)
        {
            var eachCardApplier = applier as ConditionApplierOnCardsEach;
            if (eachCardApplier is null)
                return false;
            
            var checkingCard = eachCardApplier.CurrentCheckingCard;
            if (checkingCard is null)
                return false;

            switch (conditionType)
            {
                case CardConditionType.CardType:
                    if (checkingCard.cardData.type == cardType)
                        return true;
                    break;
                case CardConditionType.OddEven:
                    if (checkingCard.cardData.number % 2 == 0 && oddEven == OddEven.Even)
                        return true;
                    
                    if (checkingCard.cardData.number % 2 != 0 && oddEven == OddEven.Odd)
                        return true;
                    break;
                case CardConditionType.NumberCombination:
                    if (numberCombination.Contains(checkingCard.cardData.number))
                        return true;
                    
                    break;
            }

            return false;
        }
    }
}
