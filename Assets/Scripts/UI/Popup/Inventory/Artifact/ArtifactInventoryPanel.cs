using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TSoft.Data;
using TSoft.Data.Card;
using TSoft.Data.Registry;
using TSoft.Item;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.UI.Popup.Inventory.Artifact
{
    public class ArtifactInventoryPanel : MonoBehaviour, IDropHandler, IInventoryUpdateHandler
    {
        [SerializeField] private Transform scrollContent;
        [SerializeField] private ItemInfoPopup info;
        [SerializeField] private GameObject slotPrefab;
        
        [Header("test")]
        [SerializeField] private List<ArtifactInfo> testItems;
        
        private List<InventoryItemSlot> inventoryItemSlots = new();
        public void UpdateSlots()
        {
            if (inventoryItemSlots.Count > 0)
            {
                foreach (var itemSlot in inventoryItemSlots)
                {
                    Destroy(itemSlot.gameObject);
                }

                inventoryItemSlots = new();
            }

            foreach (var kvp in DataRegistry.Instance.ArtifactRegistry.assetGuidLookup)
            {
                if (!GameSave.Instance.HasItemsId(kvp.Key)) 
                    continue;
                
                if (GameSave.Instance.ArtifactEquippedDictionary.Values.Contains(kvp.Key))
                    continue;
                    
                var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<InventoryItemSlot>();
                    
                slot.InitSlot(kvp.Value);
                slot.icon.itemIcon.onPointerEnter = OnItemPointerEnter;
                slot.icon.itemIcon.onPointerExit = OnItemPointerExit;
                    
                inventoryItemSlots.Add(slot);
            }
            
            if (!testItems.IsNullOrEmpty())
            {
                foreach (var item in testItems)
                {
                    if (GameSave.Instance.ArtifactEquippedDictionary.Count > 0 
                        && GameSave.Instance.ArtifactEquippedDictionary.Values.Contains(item.Id.Value))
                        continue;
                    
                    var skill = DataRegistry.Instance.ArtifactRegistry.Get(item.Id.Value);
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<InventoryItemSlot>();
                    slot.InitSlot(skill);
                    inventoryItemSlots.Add(slot);
                }
            }
        }
        
        private void OnItemPointerEnter(ItemInfo itemData, Vector2 position)
        {
            info.InitPopup(itemData);
            info.ShowPanel(position);
        }
        
        private void OnItemPointerExit()
        {
            info.HidePanel();
        }

        public void OnDrop(PointerEventData eventData)
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

            if (GameSave.Instance.ArtifactEquippedDictionary.Count > 0)
            {
                int index = -1;
                foreach (var kvp in GameSave.Instance.ArtifactEquippedDictionary)
                {
                    if (slot.itemData.Id.Value == kvp.Value)
                    {
                        index = kvp.Key;
                        break;
                    }
                }
                
                GameSave.Instance.SaveUnEquippedArtifact(index);
            }
            
            var popup = PopupContainer.Instance.GetCurrentPopup() as InventoryPopup;

            if (popup != null) 
                popup.onUpdatePopup?.Invoke();
        }
    }
}
