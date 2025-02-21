using TSoft.Data.Condition;
using UnityEngine;

namespace Data.Condition
{
    public class JokerUsedCustomCondition : CustomCondition
    {
        public override bool Interpret(ConditionApplier applier)
        {
            var player = applier.director.Player;
            if(player == null)
                return false;

            return player.JokerUsedNumber > 0;
        }
    }
}
