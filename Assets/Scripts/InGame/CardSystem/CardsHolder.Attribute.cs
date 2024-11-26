using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace TSoft.InGame.CardSystem
{
    public partial class CardsHolder
    {
        [TableList]
        public List<DefaultCardAttribute> defaultAttributes;
        
        private readonly List<AttributeState> Attributes = new ();
        
        private void InitializeAttributes()
        {
            foreach (var defaultAttribute in defaultAttributes)
            {
                var value = new CardAttributeValue
                {
                    BaseValue = defaultAttribute.value,
                    CurrentValue = defaultAttribute.value
                };
                
                CardAttributeModifier modifier = default;
                modifier.SetDefault();
                
                Attributes.Add(new AttributeState
                {
                    attrType = defaultAttribute.attrType,
                    value = value,
                    modifier = modifier
                });
            }
        }
        
        public float GetAttr(CardAttr attribute, bool current = true)
        {
            if(!TryGetAttrIndex(attribute, out var index))
                return 0.0f;
            
            return current ? Attributes[index].value.CurrentValue : Attributes[index].value.BaseValue;
        }
        
        public bool TryGetAttr(CardAttr attribute, out float value, bool current = true)
        {
            if(!TryGetAttrIndex(attribute, out var index))
            {
                value = 0.0f;
                return false;
            }
            
            value = current ? Attributes[index].value.CurrentValue : Attributes[index].value.BaseValue;
            return true;
        }
        
        public void SetAttr(CardAttr attribute, float value, bool current = true)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;

            var state = Attributes[index];
            
            if(current)
                state.value.CurrentValue = value;
            else
                state.value.BaseValue = value;
            
            Attributes[index] = state;
        }
        
        public void ClearModifiers()
        {
            for(int i = 0; i < Attributes.Count; ++i)
            {
                var state = Attributes[i];
                state.modifier.SetDefault();
                Attributes[i] = state;
            }
        }

        public void ClearModifier(CardAttr attribute)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;

            var state = Attributes[index];
            state.modifier.SetDefault();
            Attributes[index] = state;
        }
        
        public void CombineModifier(CardAttr attribute, CardAttributeModifier modifier)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;
            
            var state = Attributes[index];
            state.modifier.Combine(modifier);
            Attributes[index] = state;
        }
        
        public bool TryGetAttrIndex(CardAttr attribute, out int index)
        {
            for (int i = 0; i < Attributes.Count; ++i)
            {
                if (Attributes[i].attrType == attribute)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }
    }
}
