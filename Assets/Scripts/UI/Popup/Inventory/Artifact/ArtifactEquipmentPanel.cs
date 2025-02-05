using UI.Popup.Inventory;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory.Artifact
{
    public class ArtifactEquipmentPanel : EquipmentPanel
    {
        [SerializeField] private GameObject equipSlotPrefab;

        private const int DefaultSlotNumber = 5;
    }
}
