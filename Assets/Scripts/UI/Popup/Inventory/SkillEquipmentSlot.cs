using TMPro;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.InGame;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillEquipmentSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image slotBackgroundImage;
        [SerializeField] private GameObject iconPrefab;
        [SerializeField] private CardPatternType cardPatternType;
        
        private SkillSlotItemIcon icon;
        private ItemSO skillItem;
        
        public void UpdateIcon()
        {
            if (GameSave.Instance.SkillEquippedDictionary.TryGetValue((int)cardPatternType, out var skillId))
            {
                if(!icon)
                    icon = Instantiate(iconPrefab, transform).GetComponent<SkillSlotItemIcon>();
                
                skillItem = DataRegistry.Instance.SkillRegistry.Get(skillId);
                
                slotBackgroundImage.gameObject.SetActive(true);
                
                icon.SetSlotIcon(skillItem);
            }
            else
            {
                slotBackgroundImage.gameObject.SetActive(false);
                
                if(icon)
                    Destroy(icon.gameObject);
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
            
            //add to gamesave equipped item
            GameSave.Instance.SaveEquippedSkill(cardPatternType, slot.itemData.RegistryId.Guid);

            var skillPopup = PopupContainer.Instance.GetCurrentPopup() as SkillInventoryPopup;

            if (skillPopup != null) 
                skillPopup.onUpdatePopup?.Invoke();
        }
    }
}
