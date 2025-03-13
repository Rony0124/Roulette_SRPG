using TSoft.Data;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
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
