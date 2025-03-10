using System.Collections.Generic;
using TSoft.Data.Registry;
using TSoft.InGame;
using TSoft.Managers;
using UI.Popup.Inventory.Skill;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Sirenix.Utilities;
using TSoft.Data;

namespace TSoft.UI.Popup.Inventory
{
    public class ArtifactInventoryPanel : MonoBehaviour, IDropHandler, IInventoryUpdateHandler
    {
        [SerializeField] private Transform scrollContent;
        [SerializeField] private GameObject slotPrefab;
        
        [Header("test")] 
        [SerializeField] private List<DataRegistryIdSO> testIds;
        
        private List<InventoryItemSlot> inventoryItemSlots = new();
        public void UpdateSlots()
        {
            var items = DataRegistry.Instance.ArtifactRegistry.Ids;
            
            if (inventoryItemSlots.Count > 0)
            {
                foreach (var itemSlot in inventoryItemSlots)
                {
                    Destroy(itemSlot.gameObject);
                }

                inventoryItemSlots = new();
            }

            foreach (var itemId in items)
            {
                if (GameSave.Instance.HasItemsId(itemId.Guid))
                {
                    if (GameSave.Instance.ArtifactEquippedDictionary.Values.Contains(itemId.Guid))
                    {
                        continue;
                    }
                 
                    var skill = DataRegistry.Instance.ArtifactRegistry.Get(itemId.Guid);
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<InventoryItemSlot>();
                    
                    slot.InitSlot(skill);
                    inventoryItemSlots.Add(slot);
                }
            }
            
            if (!testIds.IsNullOrEmpty())
            {
                foreach (var id in testIds)
                {
                    if (GameSave.Instance.ArtifactEquippedDictionary.Count > 0 
                        && GameSave.Instance.ArtifactEquippedDictionary.Values.Contains(id.Guid))
                        continue;
                    
                    var skill = DataRegistry.Instance.ArtifactRegistry.Get(id.Guid);
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<InventoryItemSlot>();
                    slot.InitSlot(skill);
                    inventoryItemSlots.Add(slot);
                }
            }
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
                    if (slot.itemData.RegistryId.Guid == kvp.Value)
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
