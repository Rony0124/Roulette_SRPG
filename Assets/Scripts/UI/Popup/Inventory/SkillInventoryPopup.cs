using System;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillInventoryPopup : PopupView
    {
        public Action onUpdatePopup;

        [SerializeField] private SkillEquipmentPanel equipPanel;
        [SerializeField] private SkillInventoryPanel inventoryPanel;
        
        protected override void OnActivated()
        {
            base.OnActivated();

            equipPanel.UpdateSlots();
            inventoryPanel.UpdateSlots();
            
            onUpdatePopup += UpdateSkillPopup;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            
            onUpdatePopup -= UpdateSkillPopup;
        }
        
        private void UpdateSkillPopup()
        {
            equipPanel.UpdateSlots();
            inventoryPanel.UpdateSlots();
        }
    }
}

