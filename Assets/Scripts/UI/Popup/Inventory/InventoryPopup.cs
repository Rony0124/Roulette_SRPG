using System;
using TSoft.Managers;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class InventoryPopup : PopupView
    {
        public Action onPopupOpen;
        public Action onPopupClose;
        public Action onUpdatePopup;
        
        public enum InventoryButton
        {
            CancelButton
        }

        protected IInventoryUpdateHandler[] inventoryPanels;

        private void Awake()
        {
            Bind<Button>(typeof(InventoryButton));
            
            Get<Button>((int)InventoryButton.CancelButton).onClick.AddListener(OnCancelClicked);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            inventoryPanels = gameObject.GetComponentsInChildren<IInventoryUpdateHandler>();

            UpdatePopup();
            
            onUpdatePopup += UpdatePopup;
            
            onPopupOpen?.Invoke();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            
            onUpdatePopup -= UpdatePopup;
            
            onPopupClose?.Invoke();
        }
        
        protected virtual void UpdatePopup()
        {
            if (inventoryPanels.Length <= 0) 
                return;
            
            foreach (var panel in inventoryPanels)
            {
                panel.UpdateSlots();
            }
        }

        private void OnCancelClicked()
        {
            PopupContainer.Instance.ClosePopupUI();
            onUpdatePopup = null;
        }
    }
}
