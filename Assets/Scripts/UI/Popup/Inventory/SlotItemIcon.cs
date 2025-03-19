using TSoft.Data;
using TSoft.Item;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class SlotItemIcon : DraggableItem
    {
        public ItemIcon itemIcon;

        [HideInInspector]
        public ItemInfo itemData;
        
        public void SetSlotIcon(ItemInfo itemData)
        {
            this.itemData = itemData;
            
            itemIcon.SetItemIcon(itemData);
        }
    }
}
