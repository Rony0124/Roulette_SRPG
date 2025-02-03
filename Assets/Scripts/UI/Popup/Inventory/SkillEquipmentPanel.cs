using System.Collections.Generic;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillEquipmentPanel : MonoBehaviour
    {
        [SerializeField] private List<SkillEquipmentSlot> skillSlots;
        
        public List<SkillEquipmentSlot> SkillSlots => skillSlots;
        
        public void UpdateSlots()
        {
            foreach (var slot in skillSlots)
            {
                slot.UpdateIcon();
            }
        }
    }
}
