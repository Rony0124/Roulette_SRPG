using System;
using System.Buffers;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TSoft.Data.Condition;
using TSoft.InGame.CardSystem;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    public partial class Gameplay
    {
        [Serializable]
        public class AppliedGameplayEffect
        {
            public GameplayEffectSO sourceEffect;
            
            [HideInInspector]
            public List<AppliedModifier> appliedModifiers;
            
            public async UniTask ApplyEffect(GameplayPolicyType currentCycle)
            {
                var lifeCycle = sourceEffect.lifeCycle;
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
                
                var modifiers = sourceEffect.gameplayEffect.modifiers;
                var effect = sourceEffect.gameplayEffect.effect;
                
                for (int i = 0; i < modifiers.Length; ++i)
                {
                    var modifier = modifiers[i];
                    float magnitude = 0f;
                    if (modifier.gameplayMagnitude.magnitudeType == MagnitudeType.None)
                    {
                        magnitude = modifier.gameplayMagnitude.magnitude;
                    }else if (modifier.gameplayMagnitude.magnitudeType == MagnitudeType.Random)
                    {
                        magnitude = modifier.gameplayMagnitude.magnitudeRandom.RandomValue;
                    }

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
                    }

                    appliedModifiers.Add(appliedModifier);
                }

                var inGameDirector = GameContext.Instance.CurrentDirector as InGameDirector;
                if (inGameDirector)
                {
                    if (sourceEffect.hasCondition)
                    {
                        await sourceEffect.conditionApplier.CheckConditionEffect(inGameDirector, this);
                    }
                    else
                    {
                        await effect.ApplyEffect(inGameDirector, this);    
                    }
                    
                }
            }

            private async UniTask UndoEffect()
            {
                var effect = sourceEffect.gameplayEffect.effect;
                
                var inGameDirector = GameContext.Instance.CurrentDirector as InGameDirector;
                if (inGameDirector)
                {
                    await effect.UndoEffect(inGameDirector, this);
                }
            }
        }
        
        public List<AppliedGameplayEffect> defaultEffects;
        
        public List<AppliedGameplayEffect> appliedEffects_Passive;
        public List<AppliedGameplayEffect> appliedEffects_OnRoundBegin;
        public List<AppliedGameplayEffect> appliedEffects_OnRoundFinished;
        public List<AppliedGameplayEffect> appliedEffects_OnTurnBegin;
        public List<AppliedGameplayEffect> appliedEffects_OnTurnFinished;
        
        private void InitializeEffect()
        {
            appliedEffects_Passive = new List<AppliedGameplayEffect>();
            appliedEffects_OnRoundBegin = new List<AppliedGameplayEffect>();
            appliedEffects_OnRoundFinished = new List<AppliedGameplayEffect>();
            appliedEffects_OnTurnBegin = new List<AppliedGameplayEffect>();
            appliedEffects_OnTurnFinished = new List<AppliedGameplayEffect>();
            
            foreach (var defaultEffect in defaultEffects)
            {
                AddEffect(defaultEffect.sourceEffect);
            }
        }

        public void AddEffect(GameplayEffectSO effect)
        {
            var appliedEffect = new AppliedGameplayEffect
            {
                sourceEffect = effect
            };

            switch (effect.lifeCycle.begin)
            {
                case GameplayPolicyType.Passive:
                    appliedEffects_Passive.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundBegin:
                    appliedEffects_OnRoundBegin.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundFinished:
                    appliedEffects_OnRoundFinished.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnBegin:
                    appliedEffects_OnTurnBegin.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnFinished:
                    appliedEffects_OnTurnFinished.Add(appliedEffect);
                    break;
            }
            
            switch (effect.lifeCycle.end)
            {
                case GameplayPolicyType.Passive:
                    appliedEffects_Passive.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundBegin:
                    appliedEffects_OnRoundBegin.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundFinished:
                    appliedEffects_OnRoundFinished.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnBegin:
                    appliedEffects_OnTurnBegin.Add(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnFinished:
                    appliedEffects_OnTurnFinished.Add(appliedEffect);
                    break;
            }
        }

        public void RemoveEffect(AppliedGameplayEffect appliedEffect)
        {
            var effect = appliedEffect.sourceEffect;
            
            switch (effect.lifeCycle.begin)
            {
                case GameplayPolicyType.Passive:
                    appliedEffects_Passive.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundBegin:
                    appliedEffects_OnRoundBegin.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundFinished:
                    appliedEffects_OnRoundFinished.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnBegin:
                    appliedEffects_OnTurnBegin.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnFinished:
                    appliedEffects_OnTurnFinished.Remove(appliedEffect);
                    break;
            }
            
            switch (effect.lifeCycle.end)
            {
                case GameplayPolicyType.Passive:
                    appliedEffects_Passive.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundBegin:
                    appliedEffects_OnRoundBegin.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnRoundFinished:
                    appliedEffects_OnRoundFinished.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnBegin:
                    appliedEffects_OnTurnBegin.Remove(appliedEffect);
                    break;
                case GameplayPolicyType.OnTurnFinished:
                    appliedEffects_OnTurnFinished.Remove(appliedEffect);
                    break;
            }
        }
    }
}
