using TSoft.Data;
using TSoft.Managers;
using TSoft.UI.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class EquipmentSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] protected PopupView popup;
        [SerializeField] protected Image slotBackgroundImage;
        [SerializeField] protected GameObject iconPrefab;
        
        protected SlotItemIcon icon;
        protected ItemSO item;
        
        public virtual void UpdateIcon()
        {
            if(!icon)
                icon = Instantiate(iconPrefab, transform).GetComponent<SlotItemIcon>();
        }
        
        public virtual void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped == null)
            {
                Debug.Log("Dropped is null");
                return;
            }
            
            var slot = dropped.GetComponent<SlotItemIcon>();
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
