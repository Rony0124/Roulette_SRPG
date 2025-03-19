using TSoft.Data;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Item
{
    public class ItemInfo : RegistryAsset
    {
        public string title;
        public string description;
        public Sprite image;
        public int price;
        
        public GameplayEffectSO effect;
    }
}
