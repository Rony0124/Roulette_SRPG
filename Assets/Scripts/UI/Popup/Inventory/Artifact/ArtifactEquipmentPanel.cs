using TSoft;
using TSoft.UI.Popup.Inventory;
using UnityEngine;

namespace UI.Popup.Inventory
{
    public class ArtifactEquipmentPanel : EquipmentPanel
    {
        [SerializeField] private GameObject equipSlotPrefab;

        private const int DefaultSlotNumber = 5;
    }
}
