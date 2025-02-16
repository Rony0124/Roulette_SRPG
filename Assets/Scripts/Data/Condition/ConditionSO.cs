using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Condition;
using TSoft.InGame;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Data.Condition
{
    public class ConditionSO : ScriptableObject
    {
        public virtual async UniTask CheckCondition(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            await UniTask.CompletedTask;
        }

        public bool CheckCondition()
        {
            return false;
        }
    }
}
