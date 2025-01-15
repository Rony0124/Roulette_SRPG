using System;
using System.Buffers;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TSoft.InGame.CardSystem;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    public partial class Gameplay
    {
        [Serializable]
        public struct DefaultEffect
        {
            public GameplayEffectSO effect;
        }
        
        public class AppliedGameplayEffect
        {
            public Gameplay source;
            public GameplayEffectSO sourceEffect;
            public AppliedAttributeModifier[] appliedModifiers;
            
            public float duration;
            
            ~AppliedGameplayEffect()
            {
                if (appliedModifiers != null)
                    ArrayPool<AppliedAttributeModifier>.Shared.Return(appliedModifiers);
            }
            
            public void CaptureModifier(Gameplay target)
            {
                if (appliedModifiers == null && sourceEffect.modifiers.Length > 0)
                    appliedModifiers = ArrayPool<AppliedAttributeModifier>.Shared.Rent(sourceEffect.modifiers.Length);

                for (int i = 0; i < sourceEffect.modifiers.Length; ++i)
                {
                    var modifier = sourceEffect.modifiers[i];

                    float magnitude = modifier.magnitude;

                    ref AppliedAttributeModifier appliedModifier = ref appliedModifiers[i];
                    appliedModifier.attrType = modifier.attrType;
                    appliedModifier.modifier.SetDefault();

                    switch (modifier.modifierOp)
                    {
                        case GameplayEffectModifier.ModifierOpType.Add:
                            appliedModifier.modifier.Add = magnitude;
                            break;
                        case GameplayEffectModifier.ModifierOpType.Multiply:
                            appliedModifier.modifier.Multiply = magnitude;
                            break;
                        case GameplayEffectModifier.ModifierOpType.Divide:
                            if (magnitude != 0.0f)
                                appliedModifier.modifier.Multiply = 1.0f / magnitude;
                            break;
                        case GameplayEffectModifier.ModifierOpType.Override:
                            appliedModifier.modifier.Override = magnitude;
                            break;
                    }
                }
            }
        }
        
        public List<DefaultEffect> defaultEffects;
        [HideInInspector] 
        public List<AppliedGameplayEffect> appliedEffects;
        
        private void InitializeEffect()
        {
            appliedEffects = new List<AppliedGameplayEffect>();
            
            foreach (var defaultEffect in defaultEffects)
            {
                ApplyEffect(defaultEffect.effect, this);
            }
        }

        public void ApplyEffect(GameplayEffectSO effect, Gameplay source)
        {
            var appliedEffect = new AppliedGameplayEffect
            {
                source = source,
                sourceEffect = effect
            };

            if (effect.duration.policy == GameplayDuration.PolicyType.HasDuration)
            {
                appliedEffect.duration = effect.duration.magnitude;
            }
            
            appliedEffect.CaptureModifier(this);
            
            if (effect.duration.policy == GameplayDuration.PolicyType.Instant)
            {
                ApplyInstantGameplayEffect(appliedEffect);
                UnapplyEffect(appliedEffect);
            }
            else if(effect.duration.policy == GameplayDuration.PolicyType.HasDuration)
            {
                ApplyDurationalGameplayEffect(appliedEffect).Forget();
            }
            else
            {
                ApplyInfiniteGameplayEffect(appliedEffect);
            }
        }
        
        public void ApplyInstantGameplayEffect(AppliedGameplayEffect appliedEffect)
        {
            foreach (var appliedModifier in appliedEffect.appliedModifiers)
            {
                float baseAttr = GetAttr(appliedModifier.attrType, false);

                Debug.Log("baseAttr" + baseAttr);
                if (float.IsNaN(appliedModifier.modifier.Override))
                {
                    Debug.Log("appliedModifier.modifier.Multiply" + appliedModifier.modifier.Multiply);
                    
                    baseAttr += appliedModifier.modifier.Add;
                    baseAttr *= appliedModifier.modifier.Multiply;
                }
                else
                {
                    baseAttr = appliedModifier.modifier.Override;
                }

                SetAttr(appliedModifier.attrType, baseAttr, false);
                
                
            }

            UpdateAttributes();
        }
        
        private async UniTaskVoid ApplyDurationalGameplayEffect(AppliedGameplayEffect appliedEffect)
        {
            ApplyInfiniteGameplayEffect(appliedEffect);
            
            await UniTask.WaitForSeconds(appliedEffect.duration);

            UnapplyEffect(appliedEffect);
        }
        
        private void ApplyInfiniteGameplayEffect(AppliedGameplayEffect appliedEffect)
        {
            var effect = appliedEffects.Find(effect => effect == appliedEffect);
            if(effect != null)
                UnapplyEffect(effect);

            appliedEffects.Add(appliedEffect);
            
            UpdateAttributes();
        }
        
        private void UnapplyEffect(AppliedGameplayEffect appliedEffect)
        {
            appliedEffects.Remove(appliedEffect);
            
            UpdateAttributes();
        }
        
        public void ApplyEffectSelf(GameplayEffectSO effect, float level = 0)
        {
            ApplyEffect(effect, this);
        }
    }
}
