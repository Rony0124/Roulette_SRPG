using System.Collections.Generic;
using System.Linq;
using TSoft.InGame;

namespace TSoft.Data.Condition
{
    public class SelectedCardsCustomCondition : CustomCondition
    {
        public override bool Interpret(ConditionApplier applier)
        {
            var eachCardApplier = applier as ConditionApplierOnCards;
            if (eachCardApplier is null)
                return false;
            
            var pattern = eachCardApplier.CurrentPattern;
            if (pattern is null)
                return false;
            
            var currentSelectedCards = eachCardApplier.CurrentCards;
            if (currentSelectedCards is null || currentSelectedCards.Count <= 0)
                return false;

            var conditionType = conditionAttr.cardConditionType;

            switch (conditionType)
            {
                case CardConditionType.CardPattern:
                    return pattern.PatternType == conditionAttr.cardPatternType; 
                case CardConditionType.NumberCombination:
                    return currentSelectedCards.FindAll(card => conditionAttr.numberCombination.Contains(card.cardData.number)).Count > 0;
            }

            return false;
        }
    }
}
