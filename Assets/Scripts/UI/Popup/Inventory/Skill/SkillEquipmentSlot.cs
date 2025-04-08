using TSoft.Data.Registry;
using TSoft.InGame;
using TSoft.Managers;
using TSoft.Save;
using UI.Popup.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.UI.Popup.Inventory.Skill
{
    public class SkillEquipmentSlot : EquipmentSlot
    {
        [SerializeField] private CardPatternType cardPatternType;
        
        public override void UpdateIcon()
        {
            if (GameSave.Instance.SkillEquippedDictionary.TryGetValue((int)cardPatternType, out var skillId))
            {
                if(!icon)
                    icon = Instantiate(iconPrefab, transform).GetComponent<SlotItemIcon>();
                
                item = DataRegistry.Instance.SkillRegistry.Get(skillId);
                
                slotBackgroundImage.gameObject.SetActive(true);
                
                icon.SetSlotIcon(item);
            }
            else
            {
                slotBackgroundImage.gameObject.SetActive(false);
                
                if(icon)
                    Destroy(icon.gameObject);

                icon = null;
                item = null;
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
            GameSave.Instance.SaveEquippedSkill(cardPatternType, slot.itemData.Id.Value);

            var skillPopup = PopupContainer.Instance.GetCurrentPopup() as InventoryPopup;

            if (skillPopup != null) 
                skillPopup.onUpdatePopup?.Invoke();
        }
    }
}
