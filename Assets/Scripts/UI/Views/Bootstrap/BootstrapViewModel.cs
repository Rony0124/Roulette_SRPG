using System.Collections.Generic;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Popup.Inventory;
using UnityEngine;

namespace TSoft.UI.Views.Bootstrap
{
    public class BootstrapViewModel : ViewModelBase
    {
        public BootstrapView View => view as BootstrapView;
        private BootstrapModel Model => model as BootstrapModel;
        
        [SerializeField] 
        private GameObject artifactSlotPrefab;
        private List<ArtifactSlot> artifactSlots = new();
        
        private const int DefaultArtifactSlotCount = 5;

        private void Start()
        {
            View.optionButton.onClick.AddListener(OnOptionClicked);
            View.skillButton.onClick.AddListener(OnSkillClicked);
            View.jokerButton.onClick.AddListener(OnJokerClicked);
            View.artifactButton.onClick.AddListener(OnArtifactClicked);
            
            GameSave.Instance.onGoldChanged += View.SetGoldText;
            
            View.SetGoldText(GameSave.Instance.Gold);
            UpdateArtifactSlots();
        }
        
        private void OnOptionClicked()
        {
            Debug.Log("option clicked");
        }
        
        private void OnSkillClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Skill);
        }
        
        private void OnJokerClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Joker);
        }
        
        private void OnArtifactClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Artifact);
            
            var popup = PopupContainer.Instance.GetCurrentPopup() as InventoryPopup;

            if (popup != null)
                popup.onUpdatePopup += UpdateArtifactSlots;
        }
        
        private void UpdateArtifactSlots()
        {
            if (artifactSlots.Count > 0)
            {
                foreach (var artifactSlot in artifactSlots)
                {
                    Destroy(artifactSlot.gameObject);
                }

                artifactSlots = new();
            }
            
            var quantity = 0;
            for (var i = 0; i < DefaultArtifactSlotCount; i++)
            {
                var slot = Instantiate(artifactSlotPrefab, View.artifactsContainer).GetComponent<ArtifactSlot>();
                
                if (GameSave.Instance.ArtifactEquippedDictionary.TryGetValue(i, out var artifactId))
                {
                    var artifactData = DataRegistry.Instance.ArtifactRegistry.Get(artifactId);
                    slot.SetSlot(artifactData);

                    quantity++;
                }
                
                artifactSlots.Add(slot);
            }

            View.SetQuantityText(quantity, DefaultArtifactSlotCount);
        }

        private void OnDestroy()
        {
            GameSave.Instance.onGoldChanged -= View.SetGoldText;
        }
    }
}
