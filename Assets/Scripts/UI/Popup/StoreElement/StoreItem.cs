using System;
using TSoft.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Popup.StoreElement
{
    public class StoreItem : MonoBehaviour
    {
        public Action OnSelect;
        
        //ui
        private Button button;
        private Image thumbnail;
        //info
        private ItemSO itemInfo;
        private Guid id;
        private StorePopup.ItemType type;

        public int Price => itemInfo.price;
        public Guid Id => id;
        public StorePopup.ItemType Type => type;
        
        private void Start()
        {
            button = GetComponent<Button>();
            thumbnail = GetComponent<Image>();
            
            button.onClick.AddListener(() => OnSelect?.Invoke());
        }

        public void SetElement(ItemSO item, Guid id, StorePopup.ItemType type)
        {
            thumbnail.sprite = item.image;
            this.id = id;
            this.type = type;
        }
    }
}
