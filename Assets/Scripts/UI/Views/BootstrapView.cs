using System;
using System.Collections.Generic;
using TMPro;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Core;
using TSoft.UI.Popup.Inventory;
using UI.Views;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views
{
    public class BootstrapView : ViewBase
    {
        public static BootstrapView Instance;
        
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

        public ItemInfoPopup itemInfo;
        
        private void Awake()
        {
            Instance = this;
            
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
                
                if (GameSave.Instance.ArtifactEquippedDictionary.TryGetValue(i, out var artifactId))
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
        
        private void CalcMousePosition()
        {
            // 텍스트 라벨을 가져온다.
            var infoPanel = itemInfo;
            RectTransform rt = infoPanel.transform as RectTransform;

            var canvasRt = GetComponent<Canvas>().transform as RectTransform;
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
       
            // 마우스 좌표를 canvas내에서의 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, mousePosition, Camera.main, out var localPosition);

            //툴팁이 마우스 기준 우측에 나오는것을 고려하여 마우스의 위치가 가로길이 기준 75%를 초과하면 마우스기준 우측으로 나타나도록 한다.
            //계산식은 현재 나타난 텍스트UI의 가로길이만큼을 빼주며, *0.5f는 Scale이 0.5f로 설정되어있기때문에 사용
            if (mousePosition.x > Screen.width * 0.75f)
            {
                localPosition.x -= rt.sizeDelta.x;
            }

            // 위치 변경
            if (rt != null) 
                rt.anchoredPosition = localPosition;
        }
    }
}
