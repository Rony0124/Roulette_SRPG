using TSoft.Data;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class InventoryItemSlot : MonoBehaviour
    {
        [SerializeField] private GameObject iconPrefab;

        private ItemSO item;
        public SlotItemIcon icon { get; private set; }
        
        public void InitSlot(ItemSO itemData)
        {
            item = itemData;
            
            icon = Instantiate(iconPrefab, transform).GetComponent<SlotItemIcon>();
            icon.SetSlotIcon(item);
        }
    }
}
