using System;
using Sirenix.OdinInspector;
using TSoft.InGame;
using UnityEngine;

namespace TSoft.Data.Condition
{
    [Serializable]
    public struct ConditionAttr
    {
        public ConditionType conditionType;
        
        [ShowIf("conditionType", ConditionType.Card)]
        public CardConditionType cardConditionType;
        
        [ShowIf("cardConditionType", CardConditionType.OddEven)]
        public OddEven oddEven;
        
        [ShowIf("cardConditionType", CardConditionType.NumberCombination)]
        public int[] numberCombination;
        
        [ShowIf("cardConditionType", CardConditionType.CardType)]
        public CardType cardType;
        
        [ShowIf("cardConditionType", CardConditionType.CardPattern)]
        public CardPatternType cardPatternType;

        [ShowIf("conditionType", ConditionType.Random)]
        public float randomMagnitude;
    }
    
    public abstract class CustomCondition : IConditionExpression
    {
        public ConditionAttr conditionAttr { get; set; }
        public abstract bool Interpret(ConditionApplier applier);
    }
}
