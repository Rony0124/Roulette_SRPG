using System;
using System.Buffers;
using System.Collections.Generic;
using TSoft.InGame.CardSystem;

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
            
            public void CaptureModifier(Gameplay targetActor)
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
        public List<AppliedGameplayEffect> appliedEffects;
        
        private void InitializeEffect()
        {
            foreach (var defaultEffect in defaultEffects)
            {
                ApplyEffect(defaultEffect.effect, this);
            }
        }

        public void ApplyEffect(GameplayEffectSO effect, Gameplay source)
        {
            AppliedGameplayEffect appliedEffect = new AppliedGameplayEffect();
            appliedEffect.source = source;
            appliedEffect.sourceEffect = effect;
            
            appliedEffect.CaptureModifier(this);

            ApplyGameplayEffect(appliedEffect);
        }
        
        public void ApplyGameplayEffect(AppliedGameplayEffect appliedEffect)
        {
            appliedEffects.Add(appliedEffect);
            
            UpdateAttributes();
        }
    }
}
