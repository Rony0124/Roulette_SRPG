using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public class SpecialCardData
    {
        public string Title;
        public string Description;
        public Sprite Image;
        public CardType Type;

        public SpecialCardData(string title, string description, Sprite image, CardType type)
        {
            Title = title;
            Description = description;
            Image = image;
            Type = type;
        }

        public SpecialCardData Clone()
        {
            return new SpecialCardData(Title, Description, Image, Type);
        }
    }
}
