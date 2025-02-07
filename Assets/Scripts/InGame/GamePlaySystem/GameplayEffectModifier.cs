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
        
        public float magnitude;
    }
    
    [Serializable]
    public class Modifier
    {
        public ModifierOpType modifierOp;
        
        public float magnitude;
    }
}
