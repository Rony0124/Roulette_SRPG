using System;
using UnityEngine;

namespace InGame.CardSystem
{
    [Serializable]
    public class CardData
    {
        public string Title;
        public string Description;
        public int Cost;
        public bool IsTargetable;
        public Sprite Image;

        public CardData(string Title, string Description, int Cost, Sprite Image, bool IsTargetable)
        {
            this.Title = Title;
            this.Description = Description;
            this.Cost = Cost;
            this.IsTargetable = IsTargetable;
            this.Image = Image;
        }

        public CardData Clone()
        {
            return new CardData(Title, Description, Cost, Image, IsTargetable);
        }
    }
}
