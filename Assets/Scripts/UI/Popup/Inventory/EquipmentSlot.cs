using TSoft.Data;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class EquipmentSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] protected Image slotBackgroundImage;
        [SerializeField] protected GameObject iconPrefab;
        
        protected SkillSlotItemIcon icon;
        protected ItemSO item;
        
        public virtual void UpdateIcon()
        {
            if(!icon)
                icon = Instantiate(iconPrefab, transform).GetComponent<SkillSlotItemIcon>();
        }
        
        public virtual void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped == null)
            {
                Debug.Log("Dropped is null");
                return;
            }
            
            var slot = dropped.GetComponent<SkillSlotItemIcon>();
            if (slot == null) 
            {
                Debug.Log("slot is null");
                return;
            }

            var popup = PopupContainer.Instance.GetCurrentPopup() as InventoryPopup;

            if (popup != null) 
                popup.onUpdatePopup?.Invoke();
        }
    }
}
