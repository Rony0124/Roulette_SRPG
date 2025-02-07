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
        public class AppliedGameplayEffect
        {
            public GameplayEffectSO sourceEffect;
        }
        
        public List<AppliedGameplayEffect> defaultEffects;
        
        public HashSet<AppliedGameplayEffect> appliedEffects_Passive;
        public HashSet<AppliedGameplayEffect> appliedEffects_OnRoundBegin;
        public HashSet<AppliedGameplayEffect> appliedEffects_OnRoundFinished;
        public HashSet<AppliedGameplayEffect> appliedEffects_OnTurnBegin;
        public HashSet<AppliedGameplayEffect> appliedEffects_OnTurnFinished;
        
        private void InitializeEffect()
        {
            appliedEffects_Passive = new HashSet<AppliedGameplayEffect>();
            appliedEffects_OnRoundBegin = new HashSet<AppliedGameplayEffect>();
            appliedEffects_OnRoundFinished = new HashSet<AppliedGameplayEffect>();
            appliedEffects_OnTurnBegin = new HashSet<AppliedGameplayEffect>();
            appliedEffects_OnTurnFinished = new HashSet<AppliedGameplayEffect>();
            
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
        
        public void AddEffectSelf(GameplayEffectSO effect)
        {
            AddEffect(effect);
        }
    }
}
