using System;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.UI.Views.Store;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TSoft.UI.Popup.StoreElement
{
    public class StoreItem : MonoBehaviour
    {
        public Action onSelect;
        public Action onBuyClicked;
        
        //ui
        [SerializeField] private Button button;
        [SerializeField] private Image thumbnail;
        [SerializeField] private Button buyButton;
        
        //info
        private ItemSO itemInfo;
        private Guid id;

        public int Price => itemInfo.price;
        public Guid Id => id;
        
        private void Start()
        {
            button = GetComponent<Button>();
            thumbnail = GetComponent<Image>();
            
            button.onClick.AddListener(() => onSelect?.Invoke());
            buyButton.onClick.AddListener(() => onBuyClicked?.Invoke());
            buyButton.onClick.AddListener(OnBuyClicked);
        }

        public void SetElement(ItemSO item)
        {
            itemInfo = item;
            thumbnail.sprite = item.image;
            id = item.RegistryId.Guid;
        }

        public void OnSelect()
        {
            buyButton.gameObject.SetActive(true);
        }

        public void OnDeselect()
        {
            buyButton.gameObject.SetActive(false);
        }
        
        private void OnBuyClicked()
        {
            if (!itemInfo)
                return;
            
            //save
            var price = itemInfo.price;
            if (!GameSave.Instance.HasEnoughGold(price))
                return;
            
            GameSave.Instance.AddGold(-price);
            GameSave.Instance.AddPossessItem(itemInfo.RegistryId.Guid);
        }

    }
}
