using System;
using Sirenix.OdinInspector;
using TSoft.Data;
using TSoft.Data.Condition;
using TSoft.InGame.CardSystem.CE;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    [CreateAssetMenu(fileName = "GE_", menuName = "Create GameplayEffect", order = 1)]
    public class GameplayEffectSO : ScriptableObject
    {
        [Serializable]
        public class GameplayEffect
        {
            [SerializeReference]
            public CustomEffect effect;
            public GameplayEffectModifier[] modifiers;
        }
        
        [Serializable]
        public class GameplayEffectLifeCycle
        {
            public GameplayPolicyType begin;
            public GameplayPolicyType end;
        }
        
        [Header("Policy")]
        public GameplayEffectLifeCycle lifeCycle;
        
        [Header("GameplayEffect")]
        public GameplayEffect gameplayEffect;
        
        public bool hasDuration;
        [ShowIf("hasDuration")]
        public float duration;
        
        public bool hasUnsatisfiedEffect;
        
        [ShowIf("hasUnsatisfiedEffect")]
        public GameplayEffect unsatisfiedEffect;

        [Header("Condition")]
        public bool hasCondition;

        [ShowIf("hasCondition")]
        [SerializeReference]
        public ConditionApplier conditionApplier;
    }
}
