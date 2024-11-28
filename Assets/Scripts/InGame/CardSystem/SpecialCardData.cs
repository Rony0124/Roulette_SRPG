using System;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    [Serializable]
    public class SpecialCardData
    {
        public string Title;
        public string Description;
        public Sprite Image;
        public CardType Type;
        public GameplayEffectSO Effect;

        public SpecialCardData(string title, string description, Sprite image, CardType type, GameplayEffectSO effect)
        {
            Title = title;
            Description = description;
            Image = image;
            Type = type;
            Effect = effect;
        }

        public SpecialCardData Clone()
        {
            return new SpecialCardData(Title, Description, Image, Type, Effect);
        }
    }
}
