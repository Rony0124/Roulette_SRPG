using System.Collections.Generic;
using System.Linq;
using InGame;
using TSoft.InGame;
using UnityEngine;

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
            
            var conditionType = conditionAttr.conditionType;
            if (conditionType == ConditionType.Random)
            {
                return Random.Range(0, 1) <= conditionAttr.randomMagnitude;
            }

            var cardConditionType = conditionAttr.cardConditionType;

            switch (cardConditionType)
            {
                case CardConditionType.CardPattern:
                    return pattern.PatternType == conditionAttr.cardPatternType; 
                case CardConditionType.CardType:
                    return currentSelectedCards.FindAll(card => conditionAttr.cardType == card.cardData.type).Count > 0;
                case CardConditionType.NumberCombination:
                    return currentSelectedCards.FindAll(card => conditionAttr.numberCombination.Contains(card.cardData.number)).Count > 0;
            }

            return false;
        }
    }
}
