using TSoft.Data;
using TSoft.UI.Popup.Inventory;
using UnityEngine;

namespace UI.Popup.Inventory.Skill
{
    public class SlotItemIcon : DraggableItem
    {
        public ItemIcon itemIcon;

        [HideInInspector]
        public ItemSO itemData;
        
        public void SetSlotIcon(ItemSO itemData)
        {
            this.itemData = itemData;
            
            itemIcon.SetItemIcon(itemData);
        }
    }
}
