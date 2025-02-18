using Sirenix.OdinInspector;
using TSoft.InGame;
using UnityEngine;

namespace TSoft.Data.Condition
{
    [CreateAssetMenu(fileName = "Condition", menuName = "Create Condition/SelectedCardsCondition", order = 2)]
    public class SelectedCardsCondition : ConditionSO
    {
        public enum CardsConditionType
        {
            CardPattern
        }
        
        public CardsConditionType conditionType;
        
        [ShowIf("conditionType", CardsConditionType.CardPattern)]
        public CardPatternType cardPatternType;
        
        public override bool Interpret(ConditionApplier applier)
        {
            var eachCardApplier = applier as ConditionApplierOnCards;
            if (eachCardApplier is null)
                return false;
            
            var pattern = eachCardApplier.CurrentPattern;
            if (pattern is null)
                return false;

            switch (conditionType)
            {
                case CardsConditionType.CardPattern:
                    
                    return pattern.PatternType == cardPatternType; 
            }

            return false;
        }
    }
}
