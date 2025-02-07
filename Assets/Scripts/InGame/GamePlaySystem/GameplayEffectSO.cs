using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSoft.InGame.CardSystem.CE;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "Create GameplayEffect", order = 1)]
    public class GameplayEffectSO : ScriptableObject
    {
        [Header("Modifiers")]
        public GameplayDuration duration;

        public CustomEffect effect;
        public GameplayEffectModifier[] modifiers;

        public async UniTask ApplyEffect()
        {
            List<AppliedModifier> appliedModifiers = new();
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
                await effect.ApplyEffect(inGameDirector, appliedModifiers);
            }
        }
    }
}
