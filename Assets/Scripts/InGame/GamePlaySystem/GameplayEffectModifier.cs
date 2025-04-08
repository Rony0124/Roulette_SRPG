using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace TSoft.InGame.GamePlaySystem
{
    [Serializable]
    public class GameplayEffectModifier
    {
        public GameplayAttr attrType;
        
        public ModifierOpType modifierOp;
        
        public GameplayMagnitude gameplayMagnitude;
    }

  
}
