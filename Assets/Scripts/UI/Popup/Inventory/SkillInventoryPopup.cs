using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using TSoft.Data.Registry;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillInventoryPopup : PopupView
    {
        public Action onUpdatePopup;

        [SerializeField] private SkillEquipmentPanel equipPanel;
        [SerializeField] private SkillInventoryPanel inventoryPanel;
        
        [SerializeField] private Transform scrollContent;
        [SerializeField] private GameObject slotPrefab;
        
        private List<SkillItemSlot> inventoryItemSlots;
        
        protected override void OnActivated()
        {
            base.OnActivated();

            if (inventoryItemSlots.IsNullOrEmpty())
            {
                inventoryItemSlots = new();
                CreateSlots();
            }

            onUpdatePopup += UpdateSkillPopup;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            onUpdatePopup -= UpdateSkillPopup;
        }

        private void CreateSlots()
        {
            var skills = DataRegistry.Instance.SkillRegistry.Ids;

            foreach (var skillId in skills)
            {
                if (GameSave.Instance.HasItemsId(skillId.Guid))
                {
                    var skill = DataRegistry.Instance.SkillRegistry.Get(skillId.Guid);
                    var slot = Instantiate(slotPrefab, scrollContent.transform).GetComponent<SkillItemSlot>();
                    slot.InitSlot(skill,this);
                    inventoryItemSlots.Add(slot);
                }
            }
        }

        private void UpdateSkillPopup()
        {
            equipPanel.UpdateSlots();
            inventoryPanel.UpdateSlots();
        }
    }
}

