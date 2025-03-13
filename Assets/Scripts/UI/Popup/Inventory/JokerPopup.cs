using System;
using System.Collections.Generic;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Views.Bootstrap;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class JokerPopup : PopupView
    {
        public Action onPopupOpen;
        public Action onPopupClose;
        
        public enum JokerButton
        {
            DiscardButton,
            CancelButton
        }
        
        public enum PopupTransform
        {
            JokerContent
        }
        
        [SerializeField] 
        private GameObject jokerIconPrefab;
        
        [SerializeField] 
        private ItemInfoPopup info;

        private Transform content;
        
        private List<JokerInventoryIcon> jokerIcons = new();
        private JokerInventoryIcon currentSelectedIcon;

        private void Awake()
        {
            Bind<Button>(typeof(JokerButton));
            Bind<Transform>(typeof(PopupTransform));

            content = Get<Transform>((int)PopupTransform.JokerContent);
            
            Get<Button>((int)JokerButton.DiscardButton).onClick.AddListener(OnDiscardClicked);
            Get<Button>((int)JokerButton.CancelButton).onClick.AddListener(OnCancelClicked);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (jokerIcons.Count > 0)
            {
                foreach (var jokerIcon in jokerIcons)
                {
                    Destroy(jokerIcon.gameObject);
                }

                jokerIcons = new();
            }
            
            foreach (var jokerId in DataRegistry.Instance.JokerRegistry.Ids)
            {
                if (!GameSave.Instance.HasItemsId(jokerId.Guid))
                {
                    var njokerData = DataRegistry.Instance.JokerRegistry.Get(jokerId);
                    Debug.Log(njokerData.name + "not have");
                    continue;
                }
                
                var jokerData = DataRegistry.Instance.JokerRegistry.Get(jokerId);
                var jokerIcon = Instantiate(jokerIconPrefab, content).GetComponent<JokerInventoryIcon>();
                jokerIcon.SetItemIcon(jokerData);
                
                jokerIcon.onPointerEnter = OnJokerItemPointerEnter;
                jokerIcon.onPointerExit = OnJokerItemPointerExit;
                jokerIcon.onItemClicked = OnJokerItemClicked;
                jokerIcon.onItemReleased = OnJokerItemReleased;
                
                jokerIcons.Add(jokerIcon);
            }
            
            onPopupOpen?.Invoke();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            
            foreach (var joker in jokerIcons)
            {
                Destroy(joker.gameObject);
            }

            jokerIcons = new();
            
            onPopupClose?.Invoke();
        }

        private void OnJokerItemPointerEnter(ItemSO itemData, Vector2 position)
        {
            info.InitPopup(itemData);
            info.ShowPanel(position);
        }
        
        private void OnJokerItemPointerExit()
        {
            info.HidePanel();
        }

        private void OnJokerItemClicked(JokerInventoryIcon jokerIcon)
        {
            currentSelectedIcon = jokerIcon;
        }
        
        private void OnJokerItemReleased(JokerInventoryIcon jokerIcon)
        {
            if (currentSelectedIcon == jokerIcon)
            {
                currentSelectedIcon = null;
            }
        }

        private void OnDiscardClicked()
        {
            if(!currentSelectedIcon)
                return;
            
            Destroy(currentSelectedIcon.gameObject);

            jokerIcons.Remove(currentSelectedIcon);
        }
        
        private void OnCancelClicked()
        {
            PopupContainer.Instance.ClosePopupUI();
        }
    }
}
