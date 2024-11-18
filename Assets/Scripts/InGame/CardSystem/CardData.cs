using System;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    [Serializable]
    public class CardData
    {
        public string Title;
        public string Description;
        public bool IsTargetable;
        public Sprite Image;

        public CardData(string title, string description, Sprite image, bool isTargetable)
        {
            Title = title;
            Description = description;
            Image = image;
            IsTargetable = isTargetable;
        }

        public CardData Clone()
        {
            return new CardData(Title, Description, Image, IsTargetable);
        }
    }
}
