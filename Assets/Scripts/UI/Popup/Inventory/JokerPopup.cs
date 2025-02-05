using System;
using System.Collections.Generic;
using TSoft.Data.Registry;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class JokerPopup : PopupView
    {
        public enum JokerButton
        {
            DiscardButton
        }
        
        public enum PopupTransform
        {
            JokerContent
        }
        
        [SerializeField] private GameObject jokerIconPrefab;

        private Transform content;
        
        private List<JokerInventoryIcon> jokerIcons = new();
        private JokerInventoryIcon currentSelectedIcon;

        private void Awake()
        {
            Bind<Button>(typeof(JokerButton));
            Bind<Transform>(typeof(PopupTransform));

            content = Get<Transform>((int)PopupTransform.JokerContent);
            
            Get<Button>((int)JokerButton.DiscardButton).onClick.AddListener(OnDiscardClicked);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            
            foreach (var jokerId in DataRegistry.Instance.JokerRegistry.Ids)
            {
                if (!GameSave.Instance.HasItemsId(jokerId.Guid))
                    continue;

                var jokerData = DataRegistry.Instance.JokerRegistry.Get(jokerId);
                var jokerIcon = Instantiate(jokerIconPrefab, content).GetComponent<JokerInventoryIcon>();
                jokerIcon.SetItemIcon(jokerData);

                jokerIcon.onItemClicked = OnJokerItemClicked;
                jokerIcon.onItemReleased = OnJokerItemReleased;
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            
            foreach (var joker in jokerIcons)
            {
                Destroy(joker.gameObject);
            }

            jokerIcons = new();
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
            Destroy(currentSelectedIcon.gameObject);

            jokerIcons.Remove(currentSelectedIcon);
        }
    }
}
