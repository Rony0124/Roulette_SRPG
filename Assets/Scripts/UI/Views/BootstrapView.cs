using System;
using System.Collections.Generic;
using TMPro;
using TSoft;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Core;
using TSoft.UI.Popup.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class BootstrapView : ViewBase
    {
        private enum BootstrapButton
        {
            OptionButton,
            SkillButton,
            JokerButton,
            ArtifactButton
        }
        
        private enum BootstrapTransform
        {
            ArtifactSetting
        }

        private enum BootstrapText
        {
            QuantityText,
            GoldTxt
        }

        [Header("Option")]
        private Button optionButton;
        
        [Header("Inventory")]
        private Button skillButton;
        private Button jokerButton;
        private Button artifactButton;
        
        [Header("ArtifactSetting")]
        [SerializeField] private GameObject artifactSlotPrefab;
        private Transform artifactsContainer;
        private TextMeshProUGUI quantityText;
        private List<ArtifactSlot> artifactSlots = new();
        
        private const int DefaultArtifactSlotCount = 5;

        [Header("PlayerInfo")] 
        private TextMeshProUGUI goldText;
        
        private void Awake()
        {
            Bind<Button>(typeof(BootstrapButton));
            Bind<Transform>(typeof(BootstrapTransform));
            Bind<TextMeshProUGUI>(typeof(BootstrapText));
            
            optionButton = Get<Button>((int)BootstrapButton.OptionButton);
            skillButton = Get<Button>((int)BootstrapButton.SkillButton);
            jokerButton = Get<Button>((int)BootstrapButton.JokerButton);
            artifactButton = Get<Button>((int)BootstrapButton.ArtifactButton);
            artifactsContainer = Get<Transform>((int)BootstrapTransform.ArtifactSetting);
            quantityText = Get<TextMeshProUGUI>((int)BootstrapText.QuantityText);
            goldText = Get<TextMeshProUGUI>((int)BootstrapText.GoldTxt);
            
            optionButton.onClick.AddListener(OnOptionClicked);
            skillButton.onClick.AddListener(OnSkillClicked);
            jokerButton.onClick.AddListener(OnJokerClicked);
            artifactButton.onClick.AddListener(OnArtifactClicked);
        }

        protected override void OnActivated()
        {
            UpdateArtifactSlots();
            
            GameSave.Instance.onGoldChanged += UpdateGold;
            
            UpdateGold(GameSave.Instance.Gold);
        }

        protected override void OnDeactivated()
        {
            GameSave.Instance.onGoldChanged -= UpdateGold;
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
        
        private void HideInventoryButtons()
        {
            jokerButton.gameObject.SetActive(false);
            skillButton.gameObject.SetActive(false);
            artifactButton.gameObject.SetActive(false);
        }

        private void ShowInventoryButtons()
        {
            jokerButton.gameObject.SetActive(true);
            skillButton.gameObject.SetActive(true);
            artifactButton.gameObject.SetActive(true);
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
            
            int quantity = 0;
            for (var i = 0; i < DefaultArtifactSlotCount; i++)
            {
                var slot = Instantiate(artifactSlotPrefab, artifactsContainer).GetComponent<ArtifactSlot>();
                
                if (GameSave.Instance.ArtifactEquippedSet.TryGetValue(i, out var artifactId))
                {
                    var artifactData = DataRegistry.Instance.ArtifactRegistry.Get(artifactId);
                    slot.SetSlot(artifactData);

                    quantity++;
                }
                
                artifactSlots.Add(slot);
            }

            SetQuantityText(quantity);
        }

        private void SetQuantityText(int quantity)
        {
            quantityText.text = quantity + "/"+ DefaultArtifactSlotCount;
        }

        private void UpdateGold(int gold)
        {
            goldText.text = gold.ToString();
        }
    }
}
