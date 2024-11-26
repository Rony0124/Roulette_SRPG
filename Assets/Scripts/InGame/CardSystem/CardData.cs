using System;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    [Serializable]
    public class CardData
    {
        public string Title;
        public string Description;
        public Sprite Image;
        public int Number;
        public int Damage;
        public CardType Type;

        public CardData(string title, string description, Sprite image, int damage, int number, CardType type)
        {
            Title = title;
            Description = description;
            Image = image;
            Damage = damage;
            Number = number;
            Type = type;
        }

        public CardData Clone()
        {
            return new CardData(Title, Description, Image, Damage, Number, Type);
        }
    }
}
