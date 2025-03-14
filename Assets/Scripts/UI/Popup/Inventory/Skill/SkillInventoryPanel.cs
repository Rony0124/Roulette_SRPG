using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.Data.Skill;
using TSoft.InGame;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.UI.Popup.Inventory.Skill
{
    public class SkillInventoryPanel : MonoBehaviour, IDropHandler, IInventoryUpdateHandler
    {
        [SerializeField] private Transform scrollContent;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private ItemInfoPopup info;
        
        [Header("test")] 
        [SerializeField] private List<SkillSO> testItems;
        
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

            foreach (var kvp in DataRegistry.Instance.SkillRegistry.assetGuidLookup)
            {
                if (GameSave.Instance.HasItemsId(kvp.Key))
                {
                    if (GameSave.Instance.SkillEquippedDictionary.Values.Contains(kvp.Key))
                    {
                        continue;
                    }
                    
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<InventoryItemSlot>();
                    
                    slot.InitSlot(kvp.Value);
                    slot.icon.itemIcon.onPointerEnter = OnItemPointerEnter;
                    slot.icon.itemIcon.onPointerExit = OnItemPointerExit;
                    
                    inventoryItemSlots.Add(slot);
                }
            }
            
            if (!testItems.IsNullOrEmpty())
            {
                foreach (var item in testItems)
                {
                    if (GameSave.Instance.SkillEquippedDictionary.Values.Contains(item.Id.Value))
                    {
                        continue;
                    }
                    
                    var skill = DataRegistry.Instance.SkillRegistry.Get(item.Id.Value);
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<InventoryItemSlot>();
                    slot.InitSlot(skill);
                    inventoryItemSlots.Add(slot);
                }
            }
        }
        
        private void OnItemPointerEnter(ItemSO itemData, Vector2 position)
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

            if (GameSave.Instance.SkillEquippedDictionary.Count > 0)
            {
                CardPatternType typeKey = CardPatternType.None;
                foreach (var kvp in GameSave.Instance.SkillEquippedDictionary)
                {
                    if (slot.itemData.Id.Value == kvp.Value)
                    {
                        typeKey = (CardPatternType)kvp.Key;
                        break;
                    }
                }
                
                GameSave.Instance.SaveUnEquippedSkill(typeKey);
            }
            
            var popup = PopupContainer.Instance.GetCurrentPopup() as InventoryPopup;

            if (popup != null) 
                popup.onUpdatePopup?.Invoke();
        }
    }
}
