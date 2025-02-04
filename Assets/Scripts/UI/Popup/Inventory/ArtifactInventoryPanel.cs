using TSoft.InGame;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.UI.Popup.Inventory
{
    public class ArtifactInventoryPanel : MonoBehaviour, IDropHandler, IInventoryUpdateHandler
    {
        public void UpdateSlots()
        {
            
        }

        public void OnDrop(PointerEventData eventData)
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
