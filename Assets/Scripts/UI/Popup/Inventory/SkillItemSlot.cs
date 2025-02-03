using TSoft.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillItemSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image thumbnail;

        private ItemSO itemData;
        private SkillInventoryPopup popup;
        
        public void InitSlot(ItemSO itemData, SkillInventoryPopup popup)
        {
            this.itemData = itemData; 
            thumbnail.sprite = itemData.image;
            this.popup = popup;
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped == null)
            {
                Debug.Log("Dropped is null");
                return;
            }
                
            var slot = dropped.GetComponent<SkillEquipmentSlot>();
            if (slot == null) 
            {
                Debug.Log("slot is null");
                return;
            }
            
            //add to gamesave equipped item
            GameSave.Instance.SaveEquippedSkill(slot.CardPatternType, itemData.RegistryId.Guid);
            
            popup.onUpdatePopup?.Invoke();
            
            
            /*if (RootInventory.MoveItem(sourceSlot.Item.ItemDBId, sourceSlot.RootInventory, PageIdx, SlotIdx))
            {
            }
            else
            {
                Debug.Log("Move Failed in Drop");
            }*/
            
           
        }
    }
}
