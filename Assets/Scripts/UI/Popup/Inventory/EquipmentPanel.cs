using System.Collections.Generic;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public class EquipmentPanel : MonoBehaviour, IInventoryUpdateHandler
    {
        [SerializeField] private List<EquipmentSlot> eqSlots;
        
        public virtual void UpdateSlots()
        {
            foreach (var slot in eqSlots)
            {
                slot.UpdateIcon();
            }
        }
    }
}
