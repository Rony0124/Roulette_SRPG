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
       
        /*public struct Tags
        {
            public List<GameplayTag> mustHaveTag;
            public List<GameplayTag> mustNotHaveTag;
        }

        public Tags sourceTags;
        public Tags targetTags;*/
    }
    
    [Serializable]
    public class Modifier
    {
        public ModifierOpType modifierOp;
        
        public float magnitude;
    }
}
