using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSoft.InGame;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Data.Condition
{
    [CreateAssetMenu(fileName = "Condition", menuName = "Create Condition", order = 1)]
    public class ConditionSO : ScriptableObject
    {
        
        public virtual async UniTask<bool> CheckCondition(InGameDirector director, Gameplay.AppliedGameplayEffect sourceEffect)
        {
            await UniTask.CompletedTask;
            
            return false;
        }
    }
}
