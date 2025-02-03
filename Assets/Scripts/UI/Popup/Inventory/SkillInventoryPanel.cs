using System.Collections.Generic;
using Sirenix.Utilities;
using TSoft.Data;
using TSoft.Data.Registry;
using UnityEngine;
using System.Linq;
using TSoft.InGame;
using TSoft.Managers;
using UnityEngine.EventSystems;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillInventoryPanel : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Transform scrollContent;
        [SerializeField] private GameObject slotPrefab;
        
        [Header("test")] 
        [SerializeField] private List<DataRegistryIdSO> testIds;
        
        private List<SkillInventoryItemSlot> inventoryItemSlots = new();
        
        public void UpdateSlots()
        {
            var skills = DataRegistry.Instance.SkillRegistry.Ids;
            
            if (inventoryItemSlots.Count > 0)
            {
                foreach (var itemSlot in inventoryItemSlots)
                {
                    Destroy(itemSlot.gameObject);
                }

                inventoryItemSlots = new();
            }

            foreach (var skillId in skills)
            {
                if (GameSave.Instance.HasItemsId(skillId.Guid))
                {
                    if (GameSave.Instance.SkillEquippedDictionary.Values.Contains(skillId.Guid))
                    {
                        continue;
                    }
                 
                    var skill = DataRegistry.Instance.SkillRegistry.Get(skillId.Guid);
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<SkillInventoryItemSlot>();
                    
                    slot.InitSlot(skill);
                    inventoryItemSlots.Add(slot);
                }
            }
            
            if (!testIds.IsNullOrEmpty())
            {
                foreach (var id in testIds)
                {
                    if (GameSave.Instance.SkillEquippedDictionary.Values.Contains(id.Guid))
                    {
                        continue;
                    }
                    
                    var skill = DataRegistry.Instance.SkillRegistry.Get(id.Guid);
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<SkillInventoryItemSlot>();
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
            
            var slot = dropped.GetComponent<SkillSlotItemIcon>();
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
                    if (slot.itemData.RegistryId.Guid == kvp.Value)
                    {
                        typeKey = (CardPatternType)kvp.Key;
                        break;
                    }
                }
                
                GameSave.Instance.SaveUnEquippedSkill(typeKey);
            }
            
            
            var skillPopup = PopupContainer.Instance.GetCurrentPopup() as SkillInventoryPopup;

            if (skillPopup != null) 
                skillPopup.onUpdatePopup?.Invoke();
        }
    }
}
