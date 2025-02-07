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
                await effect.sourceEffect.ApplyEffect();
            }
            
            foreach (var effect in appliedEffects_OnRoundBegin)
            {
                await effect.sourceEffect.ApplyEffect();
            }
        }
        
        public async UniTask OnRoundFinished()
        {
            foreach (var effect in appliedEffects_OnRoundFinished)
            {
                await effect.sourceEffect.ApplyEffect();
            }
        }
        
        public async UniTask OnTurnBegin()
        {
            foreach (var effect in appliedEffects_OnTurnBegin)
            {
                await effect.sourceEffect.ApplyEffect();
            }
        }
        
        public async UniTask OnTurnFinished()
        {
            foreach (var effect in appliedEffects_OnTurnFinished)
            {
                await effect.sourceEffect.ApplyEffect();
            }
        }
    }
}
