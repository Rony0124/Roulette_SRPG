using System;

namespace TSoft.InGame.GamePlaySystem
{
    [Serializable]
    public struct DefaultGamePlayAttribute
    {
        public GameplayAttr attrType;
        public float value;
    }
    
    public struct AppliedAttributeModifier
    {
        public GameplayAttr attrType;
        public GameplayAttributeModifier modifier;
    }
    
    [Serializable]
    public struct AttributeState
    {
        public GameplayAttr attrType;
        public GameplayAttributeValue value;
        public GameplayAttributeModifier modifier;
    }
    
    public struct GameplayAttributeValue
    {
        public float BaseValue;
        public float CurrentValue;
        
        public void UpdateCurrent(GameplayAttributeModifier modifier)
        {
            if (float.IsNaN(modifier.Override))
                CurrentValue = (BaseValue + modifier.Add) * modifier.Multiply;
            else
                CurrentValue = modifier.Override;
        }
    }
    
    public struct GameplayAttributeModifier
    {
        public float Add;
        public float Multiply;
        public float Override;

        public void SetDefault()
        {
            Add = 0.0f;
            Multiply = 1.0f;
            Override = float.NaN;
        }

        public void Combine(in GameplayAttributeModifier modifier)
        {
            Add += modifier.Add;
            Multiply *= modifier.Multiply;
            Override = modifier.Override;
        }
    }
}
