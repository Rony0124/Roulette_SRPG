using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    public partial class Gameplay : MonoBehaviour
    {
        public void Init()
        {
            InitializeAttributes();
            InitializeEffect();
        }

        public async UniTask OnRoundBegin()
        {
            foreach (var effect in appliedEffects_Passive)
            {
                await effect.ApplyEffect(GameplayPolicyType.Passive);
            }
            
            foreach (var effect in appliedEffects_OnRoundBegin)
            {
                await effect.ApplyEffect(GameplayPolicyType.OnRoundBegin);
            }
        }
        
        public async UniTask OnRoundFinished()
        {
            foreach (var effect in appliedEffects_OnRoundFinished)
            {
                await effect.ApplyEffect(GameplayPolicyType.OnRoundFinished);
            }
        }
        
        public async UniTask OnTurnBegin()
        {
            foreach (var effect in appliedEffects_OnTurnBegin)
            {
                await effect.ApplyEffect(GameplayPolicyType.OnTurnBegin);
            }
        }
        
        public async UniTask OnTurnFinished()
        {
            foreach (var effect in appliedEffects_OnTurnFinished)
            {
                await effect.ApplyEffect(GameplayPolicyType.OnTurnFinished);
            }
        }
    }
}
