using System.Linq;
using TSoft.InGame;
using UnityEngine;

namespace TSoft.Data.Condition
{
    public class SelectedCardCustomCondition : CustomCondition
    {
        public override bool Interpret(ConditionApplier applier)
        {
            var eachCardApplier = applier as ConditionApplierOnCardsEach;
            if (eachCardApplier is null)
                return false;
            
            var checkingCard = eachCardApplier.CurrentCheckingCard;
            if (checkingCard is null)
                return false;
            
            var conditionType = conditionAttr.cardConditionType;

            switch (conditionType)
            {
                case CardConditionType.CardType:
                    Debug.Log("checkingCard.cardData.type : " + checkingCard.cardData.type);
                    Debug.Log("cardType : " + conditionAttr.cardType);
                    
                    return checkingCard.cardData.type == conditionAttr.cardType; 
                case CardConditionType.OddEven:
                    Debug.Log("checkingCard.cardData.number : " + checkingCard.cardData.number);
                    Debug.Log("{oddEven} : " + conditionAttr.oddEven);
                    
                    if (checkingCard.cardData.number % 2 == 0 && conditionAttr.oddEven == OddEven.Even)
                        return true;
                    
                    if (checkingCard.cardData.number % 2 != 0 && conditionAttr.oddEven == OddEven.Odd)
                        return true;
                    
                    break;
                case CardConditionType.NumberCombination:
                    return conditionAttr.numberCombination.Contains(checkingCard.cardData.number);
            }

            return false;
        }
    }
}
