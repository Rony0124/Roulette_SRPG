using System;

namespace TSoft.InGame.CardSystem
{
    [Serializable]
    public struct DefaultCardAttribute
    {
        public CardAttr attrType;
        public float value;
    }
    
    [Serializable]
    public struct AttributeState
    {
        public CardAttr attrType;
        public CardAttributeValue value;
        public CardAttributeModifier modifier;
    }
    
    public struct CardAttributeValue
    {
        public float BaseValue;
        public float CurrentValue;
        
        public void UpdateCurrent(CardAttributeModifier modifier)
        {
            if (float.IsNaN(modifier.Override))
                CurrentValue = (BaseValue + modifier.Add) * modifier.Multiply;
            else
                CurrentValue = modifier.Override;
        }
    }
    
    public struct CardAttributeModifier
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

        public void Combine(in CardAttributeModifier modifier)
        {
            Add += modifier.Add;
            Multiply *= modifier.Multiply;
            Override = modifier.Override;
        }
    }
}
