using System.Linq;
using Sirenix.OdinInspector;
using TSoft.InGame;
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
            CardType,
            NumberCombination
        }
        
        public CardConditionType conditionType;

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
                    Debug.Log("checkingCard.cardData.type : " + checkingCard.cardData.type);
                    Debug.Log("cardType : " + cardType);
                    
                    return checkingCard.cardData.type == cardType; 
                case CardConditionType.OddEven:
                    Debug.Log("checkingCard.cardData.number : " + checkingCard.cardData.number);
                    Debug.Log("{oddEven} : " + oddEven);
                    
                    if (checkingCard.cardData.number % 2 == 0 && oddEven == OddEven.Even)
                        return true;
                    
                    if (checkingCard.cardData.number % 2 != 0 && oddEven == OddEven.Odd)
                        return true;
                    
                    break;
                case CardConditionType.NumberCombination:
                    return numberCombination.Contains(checkingCard.cardData.number);
            }

            return false;
        }
    }
}
