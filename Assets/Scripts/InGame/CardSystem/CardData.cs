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
        public int Damage;

        public CardData(string title, string description, Sprite image, int damage)
        {
            Title = title;
            Description = description;
            Image = image;
            Damage = damage;
        }

        public CardData Clone()
        {
            return new CardData(Title, Description, Image, Damage);
        }
    }
}
