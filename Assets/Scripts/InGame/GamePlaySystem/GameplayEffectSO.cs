using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSoft.Data;
using TSoft.InGame.CardSystem.CE;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "Create GameplayEffect", order = 1)]
    public class GameplayEffectSO : ScriptableObject
    {
        [Serializable]
        public class GameplayEffectLifeCycle
        {
            public GameplayPolicyType begin;
            public GameplayPolicyType end;
        }
        
        [Header("Policy")]
        public GameplayEffectLifeCycle lifeCycle;
        
        [Header("Effect")]
        [SerializeReference]
        public CustomEffect effect;
        public GameplayEffectModifier[] modifiers;

        [HideInInspector]
        public List<AppliedModifier> appliedModifiers;
        
        public async UniTask ApplyEffect(GameplayPolicyType currentCycle)
        {
            if (currentCycle == lifeCycle.begin)
            {
                await DoEffect();
            }

            if (currentCycle == lifeCycle.end)
            {
                await UndoEffect();
            }
        }
        
        private async UniTask DoEffect()
        {
            appliedModifiers = new();
            for (int i = 0; i < modifiers.Length; ++i)
            {
                var modifier = modifiers[i];

                float magnitude = modifier.magnitude;

                AppliedModifier appliedModifier = new()
                {
                    attrType = modifier.attrType
                };
                
                appliedModifier.modifier.SetDefault();

                switch (modifier.modifierOp)
                {
                    case ModifierOpType.Add:
                        appliedModifier.modifier.Add = magnitude;
                        break;
                    case ModifierOpType.Multiply:
                        appliedModifier.modifier.Multiply = magnitude;
                        break;
                    case ModifierOpType.Divide:
                        if (magnitude != 0.0f)
                            appliedModifier.modifier.Multiply = 1.0f / magnitude;
                        break;
                    case ModifierOpType.Override:
                        appliedModifier.modifier.Override = magnitude;
                        break;
                    case ModifierOpType.Repeater:
                        appliedModifier.modifier.Repeater = magnitude;
                        break;
                }
                
                appliedModifiers.Add(appliedModifier);
            }
            
            var inGameDirector = GameContext.Instance.CurrentDirector as InGameDirector;
            if (inGameDirector)
            {
                await effect.ApplyEffect(inGameDirector, this);
            }
        }

        private async UniTask UndoEffect()
        {
            var inGameDirector = GameContext.Instance.CurrentDirector as InGameDirector;
            if (inGameDirector)
            {
                await effect.UndoEffect(inGameDirector, this);
            }
        }
    }
}
