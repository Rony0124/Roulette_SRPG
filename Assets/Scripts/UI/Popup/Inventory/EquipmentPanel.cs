using System.Collections.Generic;
using TSoft.UI.Popup.Inventory;
using UnityEngine;

namespace UI.Popup.Inventory
{
    public class EquipmentPanel : MonoBehaviour, IInventoryUpdateHandler
    {
        [SerializeField] protected List<EquipmentSlot> eqSlots;
        
        public virtual void UpdateSlots()
        {
            foreach (var slot in eqSlots)
            {
                slot.UpdateIcon();
            }
        }
    }
}
