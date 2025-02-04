using System;

namespace TSoft.UI.Popup.Inventory
{
    public class InventoryPopup : PopupView
    {
        public Action onUpdatePopup;

        protected IInventoryUpdateHandler[] inventoryPanels;
        
        protected override void OnActivated()
        {
            base.OnActivated();

            inventoryPanels = gameObject.GetComponentsInChildren<IInventoryUpdateHandler>();

            UpdatePopup();
            
            onUpdatePopup += UpdatePopup;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            
            onUpdatePopup -= UpdatePopup;
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
    }
}
