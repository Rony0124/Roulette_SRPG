using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSoft.InGame;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Data.Condition
{
    public class ConditionSO : ScriptableObject, IConditionExpression
    {
        public virtual bool Interpret(ConditionApplier applier)
        {
            return false;
        }
    }
}
