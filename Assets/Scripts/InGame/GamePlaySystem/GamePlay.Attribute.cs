using System.Collections.Generic;
using Sirenix.OdinInspector;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    public partial class Gameplay
    {
        [TableList]
        public List<DefaultGamePlayAttribute> defaultAttributes;
        
        public List<AttributeState> Attributes;
        
        private void InitializeAttributes()
        {
            Attributes = new();
            
            foreach (var defaultAttribute in defaultAttributes)
            {
                var baseVal = new ObservableVar<float>();
                var currentVal = new ObservableVar<float>();
                
                baseVal.Value = defaultAttribute.value;
                currentVal.Value = defaultAttribute.value;
                
                var value = new GameplayAttributeValue
                {
                    BaseValue = baseVal,
                    CurrentValue = currentVal
                };

                GameplayAttributeModifier modifier = default;
                modifier.SetDefault();
                
                Attributes.Add(new AttributeState
                {
                    attrType = defaultAttribute.attrType,
                    value = value,
                    modifier = modifier
                });
            }
        }
        
        public void UpdateAttributes()
        {
            ClearModifiers();
            
            /*foreach(var appliedEffect in appliedEffects)
            {
                if (appliedEffect.appliedModifiers == null) 
                    continue;
                
                foreach (var appliedModifiers in appliedEffect.appliedModifiers)
                {
                    CombineModifier(appliedModifiers.attrType, appliedModifiers.modifier);
                }
            }*/
                
            for (int i = 0; i < Attributes.Count; ++i)
            {
                var state = Attributes[i];
                state.value.UpdateCurrent(state.modifier);
                Attributes[i] = state;
            }
            
            PostUpdateAttributes();
        }
        
        private void PostUpdateAttributes()
        {
            float maxHealth = GetAttr(GameplayAttr.MaxHeart);
            float maxEnergy = GetAttr(GameplayAttr.MaxEnergy);
            
            if(GetAttr(GameplayAttr.Heart, false) > maxHealth)
                SetAttr(GameplayAttr.Heart, maxHealth, false);
            
            if(GetAttr(GameplayAttr.Heart) > maxHealth)
                SetAttr(GameplayAttr.Heart, maxHealth);
            
            if(GetAttr(GameplayAttr.Energy, false) > maxEnergy)
                SetAttr(GameplayAttr.Energy, maxEnergy, false);
            
            if(GetAttr(GameplayAttr.Energy) > maxEnergy)
                SetAttr(GameplayAttr.Energy, maxEnergy);
            
            if (GetAttr(GameplayAttr.Heart) <= 0.0f)
            {
                SetAttr(GameplayAttr.Heart, 0.0f);
            }
        }
        
        public float GetAttr(GameplayAttr attribute, bool current = true)
        {
            if(!TryGetAttrIndex(attribute, out var index))
                return 0.0f;
            
            return current ? Attributes[index].value.CurrentValue.Value : Attributes[index].value.BaseValue.Value;
        }
        
        public ObservableVar<float> GetAttrVar(GameplayAttr attribute, bool current = true)
        {
            if(!TryGetAttrIndex(attribute, out var index))
                return null;
            
            return current ? Attributes[index].value.CurrentValue : Attributes[index].value.BaseValue;
        }
        
        public bool TryGetAttr(GameplayAttr attribute, out float value, bool current = true)
        {
            if(!TryGetAttrIndex(attribute, out var index))
            {
                value = 0.0f;
                return false;
            }
            
            value = current ? Attributes[index].value.CurrentValue.Value : Attributes[index].value.BaseValue.Value;
            return true;
        }
        
        public void SetAttr(GameplayAttr attribute, float value, bool current = true)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;

            var state = Attributes[index];
            
            if(current)
                state.value.CurrentValue.Value = value;
            else
                state.value.BaseValue.Value = value;
            
            Attributes[index] = state;
        }
        
        private void ClearModifiers()
        {
            for(int i = 0; i < Attributes.Count; ++i)
            {
                var state = Attributes[i];
                state.modifier.SetDefault();
                Attributes[i] = state;
            }
        }

        public void ClearModifier(GameplayAttr attribute)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;

            var state = Attributes[index];
            state.modifier.SetDefault();
            Attributes[index] = state;
        }
        
        public void CombineModifier(GameplayAttr attribute, GameplayAttributeModifier modifier)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;
            
            var state = Attributes[index];
            state.modifier.Combine(modifier);
            Attributes[index] = state;
        }
        
        public bool TryGetAttrIndex(GameplayAttr attribute, out int index)
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
