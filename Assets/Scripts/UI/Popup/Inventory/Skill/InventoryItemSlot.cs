using TSoft.Data;
using UI.Popup.Inventory.Skill;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class InventoryItemSlot : MonoBehaviour
    {
        [SerializeField] private GameObject iconPrefab;

        private ItemSO item;
        private SlotItemIcon icon;
        
        public void InitSlot(ItemSO itemData)
        {
            item = itemData;
            
            icon = Instantiate(iconPrefab, transform).GetComponent<SlotItemIcon>();
            icon.SetSlotIcon(item);
        }
    }
}
