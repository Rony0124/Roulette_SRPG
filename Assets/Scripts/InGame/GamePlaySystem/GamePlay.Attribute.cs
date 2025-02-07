using System.Collections.Generic;
using Sirenix.OdinInspector;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace TSoft.InGame.GamePlaySystem
{
    public partial class Gameplay
    {
        [TableList]
        public List<DefaultGamePlayAttribute> defaultAttributes;
        
        public List<AttributeState> attributes;
        [HideInInspector] 
        public List<AppliedModifier> attrAppliedModifiers;
        
        private void InitializeAttributes()
        {
            attributes = new();
            
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
                
                attributes.Add(new AttributeState
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
            
            foreach(var appliedModifier in attrAppliedModifiers)
            {
                if (appliedModifier.attrType == GameplayAttr.None) 
                    continue;
                
                CombineModifier(appliedModifier.attrType, appliedModifier.modifier);
            }
            
            for (int i = 0; i < attributes.Count; ++i)
            {
                var state = attributes[i];
                state.value.UpdateCurrent(state.modifier);
                attributes[i] = state;
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
            
            return current ? attributes[index].value.CurrentValue.Value : attributes[index].value.BaseValue.Value;
        }
        
        public ObservableVar<float> GetAttrVar(GameplayAttr attribute, bool current = true)
        {
            if(!TryGetAttrIndex(attribute, out var index))
                return null;
            
            return current ? attributes[index].value.CurrentValue : attributes[index].value.BaseValue;
        }
        
        public bool TryGetAttr(GameplayAttr attribute, out float value, bool current = true)
        {
            if(!TryGetAttrIndex(attribute, out var index))
            {
                value = 0.0f;
                return false;
            }
            
            value = current ? attributes[index].value.CurrentValue.Value : attributes[index].value.BaseValue.Value;
            return true;
        }
        
        public void SetAttr(GameplayAttr attribute, float value, bool current = true)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;

            var state = attributes[index];
            
            if(current)
                state.value.CurrentValue.Value = value;
            else
                state.value.BaseValue.Value = value;
            
            attributes[index] = state;
        }
        
        private void ClearModifiers()
        {
            for(int i = 0; i < attributes.Count; ++i)
            {
                var state = attributes[i];
                state.modifier.SetDefault();
                attributes[i] = state;
            }
        }

        public void ClearModifier(GameplayAttr attribute)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;

            var state = attributes[index];
            state.modifier.SetDefault();
            attributes[index] = state;
        }
        
        public void CombineModifier(GameplayAttr attribute, GameplayAttributeModifier modifier)
        {
            if (!TryGetAttrIndex(attribute, out var index))
                return;
            
            var state = attributes[index];
            state.modifier.Combine(modifier);
            attributes[index] = state;
        }
        
        public bool TryGetAttrIndex(GameplayAttr attribute, out int index)
        {
            for (int i = 0; i < attributes.Count; ++i)
            {
                if (attributes[i].attrType == attribute)
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
