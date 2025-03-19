using TSoft.Data;
using TSoft.Item;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class InventoryItemSlot : MonoBehaviour
    {
        [SerializeField] private GameObject iconPrefab;

        private ItemInfo item;
        public SlotItemIcon icon { get; private set; }
        
        public void InitSlot(ItemInfo itemData)
        {
            item = itemData;
            
            icon = Instantiate(iconPrefab, transform).GetComponent<SlotItemIcon>();
            icon.SetSlotIcon(item);
        }
    }
}
