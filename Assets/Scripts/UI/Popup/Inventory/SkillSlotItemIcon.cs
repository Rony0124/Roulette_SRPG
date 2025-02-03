using TSoft.Data;
using TSoft.Managers;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillSlotItemIcon : DraggableItem
    {
        public SkillItemIcon itemIcon;

        [HideInInspector]
        public ItemSO itemData;
        
        public void SetSlotIcon(ItemSO itemData)
        {
            this.itemData = itemData;
            
            itemIcon.SetItemIcon(itemData);
        }
    }
}
