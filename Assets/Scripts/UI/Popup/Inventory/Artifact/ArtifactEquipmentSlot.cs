using TSoft.Data.Registry;
using TSoft.Managers;
using UI.Popup.Inventory.Skill;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.UI.Popup.Inventory.Artifact
{
    public class ArtifactEquipmentSlot : EquipmentSlot
    {
        [SerializeField] private int currentIndex;
        public override void UpdateIcon()
        {
            currentIndex = transform.GetSiblingIndex() - 1;
            if (GameSave.Instance.ArtifactEquippedDictionary.TryGetValue(currentIndex, out var id))
            {
                if(!icon)
                    icon = Instantiate(iconPrefab, transform).GetComponent<SlotItemIcon>();
                
                item = DataRegistry.Instance.ArtifactRegistry.Get(id);
                
                slotBackgroundImage.gameObject.SetActive(true);
                
                icon.SetSlotIcon(item);
            }
            else
            {
                slotBackgroundImage.gameObject.SetActive(false);
                
                if(icon)
                    Destroy(icon.gameObject);
            }
        }
        
        public override void OnDrop(PointerEventData eventData)
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
            
            //add to gamesave equipped item
            GameSave.Instance.SaveEquippedArtifact(currentIndex, slot.itemData.RegistryId.Guid);

            var popup = PopupContainer.Instance.GetCurrentPopup() as InventoryPopup;

            if (popup != null) 
                popup.onUpdatePopup?.Invoke();
        }
    }
}
