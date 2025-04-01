using InGame;
using Sirenix.OdinInspector;
using TSoft.Data;
using UnityEngine;

namespace HF.Data.Card
{
    [CreateAssetMenu(fileName = "Card", menuName = "Data/CardData", order = 0)]
    public class CardData : RegistryAsset
    {
        [Header("Display")]
        public string title;
        public Sprite artUI;
        public Sprite artInGame;
   
        [Header("Stats")]
        public CardType type;
        public int number;
    }
}
