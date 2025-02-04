using TSoft.Data;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillInventoryItemSlot : MonoBehaviour
    {
        [SerializeField] private GameObject iconPrefab;

        private ItemSO item;
        private SkillSlotItemIcon icon;
        
        public void InitSlot(ItemSO itemData)
        {
            item = itemData;
            
            icon = Instantiate(iconPrefab, transform).GetComponent<SkillSlotItemIcon>();
            icon.SetSlotIcon(item);
        }
    }
}
