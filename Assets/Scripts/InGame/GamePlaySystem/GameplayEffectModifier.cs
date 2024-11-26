using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace TSoft.InGame.GamePlaySystem
{
    [Serializable]
    public class GameplayEffectModifier
    {
        public GameplayAttr attrType;
                
        public enum ModifierOpType
        {
            Add,
            Multiply,
            Divide,
            Override,
        }
        
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
}
