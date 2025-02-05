using System;
using System.Collections.Generic;
using System.Linq;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Core;
using TSoft.UI.Popup;
using TSoft.UI.Popup.StoreElement;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TSoft.UI.Views.Store
{
    public class StoreView : ViewBase
    {
        private enum TransformParent
        {
            Artifact,
            Joker,
            Skill
        }
        
        private enum StoreButton
        {
            BackButton,
            SkillButton,
            JokerButton,
            ArtifactButton
        }

        [SerializeField] 
        private GameObject artifactDisplayPrefab;
        [SerializeField] 
        private GameObject jokerDisplayPrefab;
        [SerializeField] 
        private GameObject skillDisplayPrefab;
        
        private Transform artifactDisplayParent;
        private Transform jokerDisplayParent;
        private Transform skillDisplayParent;
        private Button backButton;
        private Button skillButton;
        private Button jokerButton;
        private Button artifactButton;

        private StoreItem currentStoreItem;
        private List<StoreItem> items;
        
        private const int MaxDisplayNumber = 3;

        private void Awake()
        {
            items = new();
            
            Bind<Transform>(typeof(TransformParent));
            Bind<Button>(typeof(StoreButton));
            
            artifactDisplayParent = Get<Transform>((int)TransformParent.Artifact);
            jokerDisplayParent = Get<Transform>((int)TransformParent.Joker);
            skillDisplayParent = Get<Transform>((int)TransformParent.Skill);

            backButton = Get<Button>((int)StoreButton.BackButton);
            skillButton = Get<Button>((int)StoreButton.SkillButton);
            jokerButton = Get<Button>((int)StoreButton.JokerButton);
            artifactButton = Get<Button>((int)StoreButton.ArtifactButton);
            
            backButton.onClick.AddListener(OnExitClicked);
            skillButton.onClick.AddListener(OnSkillClicked);
            jokerButton.onClick.AddListener(OnJokerClicked);
            artifactButton.onClick.AddListener(OnArtifactClicked);
        }

        protected override void OnActivated()
        {
            CreateDisplayItems();
        }

        protected override void OnDeactivated()
        {
        }
        
        private void CreateDisplayItems()
        {
            CreateDisplay(
                DataRegistry.Instance.JokerRegistry,
                jokerDisplayPrefab,
                jokerDisplayParent
            );

            CreateDisplay(
                DataRegistry.Instance.ArtifactRegistry,
                artifactDisplayPrefab,
                artifactDisplayParent
            );

            CreateDisplay(
                DataRegistry.Instance.SkillRegistry,
                skillDisplayPrefab,
                skillDisplayParent
            );
        }

        private void CreateDisplay<T>(RegistrySO<T> registry, GameObject prefab, Transform parent)
            where T : ItemSO
        {
            int displayCount = 0;
            foreach (var itemId in registry.Ids)
            {
                if (GameSave.Instance.HasItemsId(itemId.Guid))
                {
                    continue;
                }
                
                displayCount++;
            }

            var uniqueNumbers = GetUniqueNumbers(displayCount);

            for (int i = 0; i < uniqueNumbers.Count; i++)
            {
                if (!registry.TryGetKvpByIndex(uniqueNumbers[i], out var kvp))
                {
                    continue;
                }

                var obj = Instantiate(prefab, parent);
                var storeItem = obj.GetComponent<StoreItem>();
                var info = kvp.Value;

                storeItem.onSelect = () =>
                {
                    OnSelect(storeItem);
                };

                storeItem.onBuyClicked = OnBuyClicked;

                storeItem.SetElement(info);
                items.Add(storeItem);
            }
        }

        private void OnSelect(StoreItem item)
        {
            if (currentStoreItem)
            {
                currentStoreItem.OnDeselect();
            }
            
            currentStoreItem = item;

            item.OnSelect();
        }
        
        private void OnBuyClicked()
        {
            if (!currentStoreItem)
                return;
            
            //save
            var price = currentStoreItem.Price;
            if (!GameSave.Instance.HasEnoughGold(price))
                return;
            
            GameSave.Instance.AddGold(-price);
            GameSave.Instance.AddPossessItem(currentStoreItem.Id);
            
            //apply
            items.Remove(currentStoreItem);
            
            //delete
            Destroy(currentStoreItem.gameObject);
            currentStoreItem = null;
        }
        
        private void OnExitClicked()
        {
            if (PopupContainer.Instance.GetCurrentPopup() != null)
            {
                PopupContainer.Instance.ClosePopupUI();
                
                ShowInventoryButtons();
            }
            else
            {
                SceneManager.LoadScene(Define.StageMap);    
            }
        }
        
        private List<int> GetUniqueNumbers(int count)
        {
            if(count == 0)
                return new List<int>();
            
            var uniqueNumbers = new HashSet<int>();
            var random = new System.Random();
            var min = Mathf.Min(count, MaxDisplayNumber);
            
            while (uniqueNumbers.Count < min)
            {
                int number = random.Next(0, count);
                uniqueNumbers.Add(number);
            }

            return uniqueNumbers.ToList();
        }
        
        private void OnSkillClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Skill);

            HideInventoryButtons();
        }
        
        private void OnJokerClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Joker);
            
            HideInventoryButtons();
        }
        
        private void OnArtifactClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Artifact);
            
            HideInventoryButtons();
        }

        private void HideInventoryButtons()
        {
            jokerButton.gameObject.SetActive(false);
            skillButton.gameObject.SetActive(false);
            artifactButton.gameObject.SetActive(false);
            
            if (currentStoreItem)
            {
                currentStoreItem.OnDeselect();
            }

            currentStoreItem = null;
        }

        private void ShowInventoryButtons()
        {
            jokerButton.gameObject.SetActive(true);
            skillButton.gameObject.SetActive(true);
            artifactButton.gameObject.SetActive(true);
        }
    }
}
