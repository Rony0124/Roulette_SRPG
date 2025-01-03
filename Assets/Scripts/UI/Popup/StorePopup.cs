using System.Collections.Generic;
using System.Linq;
using TMPro;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.InGame.Player;
using TSoft.UI.Popup.StoreElement;
using TSoft.Utils;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Popup
{
    public class StorePopup : PopupView
    {
        public enum ItemType
        {
            Joker,
            Artifact
        }
        
        private enum DisplayParent
        {
            Artifacts_Display,
            Jokers_Display
        }
        
        private enum InventoryParent
        {
            Artifacts_Inventory,
            Jokers_Inventory
        }
        
        private enum Text
        {
            DescriptionText
        }
        
        private enum StoreButton
        {
            BuyButton,
            SellButton
        }

        [Header("Display")]
        [SerializeField] 
        private GameObject artifactDisplayPrefab;
        [SerializeField] 
        private GameObject jokerDisplayPrefab;
        
        [Header("Inventory")]
        [SerializeField] 
        private GameObject artifactInventoryPrefab;
        [SerializeField] 
        private GameObject jokerInventoryPrefab;
        
        [SerializeField] 
        private GameObject player;
        
        //ui
        private Transform artifactDisplayParent;
        private Transform jokerDisplayParent;
        private Transform artifactInventoryParent;
        private Transform jokerInventoryParent;
        private TextMeshProUGUI descriptionText;
        private Button sellButton;
        
        //item
        private List<StoreItem> artifactsStore = new();
        private List<StoreItem> jokersStore = new();
        private List<InventoryItem> artifactsInventory = new();
        private List<InventoryItem> jokersInventory = new();

        private StoreItem currentStoreItem;

        private StoreItem CurrentStoreItem
        {
            get => currentStoreItem;
            set
            {
                sellButton.enabled = value != null;
                currentStoreItem = value;
            }
        }
        private InventoryItem currentInventoryItem;
        
        private const int DisplayNumber = 3;
        
        private void Awake()
        {
            Bind<Transform>(typeof(DisplayParent));
            Bind<Transform>(typeof(InventoryParent));
            Bind<TextMeshPro>(typeof(Text));
            Bind<Button>(typeof(StoreButton));
            
            artifactDisplayParent = Get<Transform>((int)DisplayParent.Artifacts_Display);
            jokerDisplayParent = Get<Transform>((int)DisplayParent.Jokers_Display);
            artifactInventoryParent = Get<Transform>((int)InventoryParent.Artifacts_Inventory);
            jokerInventoryParent = Get<Transform>((int)InventoryParent.Jokers_Inventory);
            descriptionText = Get<TextMeshProUGUI>((int)Text.DescriptionText);
            sellButton = Get<Button>((int)StoreButton.SellButton);
            
            sellButton.onClick.AddListener(OnBuyClicked);
        }

        protected override void OnActivated()
        {
            CreateDisplayItems();
            CreateInventoryItems();
        }
        
        protected override void OnDeactivated()
        {
            ClearAllItems();
        }

        private void CreateDisplayItems()
        {
            var artifactNums = GetUniqueNumbers(DataRegistry.Instance.ArtifactRegistry.Count);
            var jokerNums = GetUniqueNumbers(DataRegistry.Instance.JokerRegistry.Count);

            for (int i = 0; i < DisplayNumber; i++)
            {
                if (!DataRegistry.Instance.ArtifactRegistry.TryGetKvpByIndex(artifactNums[i], out var kvp))
                {
                    continue;
                }
                
                var obj = Instantiate(artifactDisplayPrefab, artifactDisplayParent);
                var artifact = obj.GetComponent<StoreItem>();
                var infoId = kvp.Key;
                var info = kvp.Value;
                
                artifact.OnSelect = () =>
                {
                    CurrentStoreItem = artifact;
                    UpdateSelectedItem(info);
                };
                
                artifact.SetElement(info, infoId, ItemType.Artifact);
                artifactsStore.Add(artifact);
            }
            
            for (int i = 0; i < DisplayNumber; i++)
            {
                if (!DataRegistry.Instance.JokerRegistry.TryGetKvpByIndex(jokerNums[i], out var kvp))
                {
                    continue;
                }
                
                var obj = Instantiate(jokerDisplayPrefab, jokerDisplayParent);
                var joker = obj.GetComponent<StoreItem>();
                var infoId = kvp.Key;
                var info = kvp.Value;
                
                joker.OnSelect = () =>
                {
                    CurrentStoreItem = joker;
                    UpdateSelectedItem(info);
                };
                
                joker.SetElement(info, infoId, ItemType.Joker);
                jokersStore.Add(joker);
            }
        }

        private void CreateInventoryItems()
        {
            var artifactIds = DataRegistry.Instance.ArtifactRegistry.Ids;
            var jokerIds = DataRegistry.Instance.JokerRegistry.Ids;

            foreach (var artifactId in artifactIds)
            {
                if (!GameSave.Instance.HasItemsId(artifactId.Guid))
                {
                    continue;
                }

                var data = DataRegistry.Instance.ArtifactRegistry.Get(artifactId);
                   
                var obj = Instantiate(artifactInventoryPrefab, artifactInventoryParent);
                var artifact = obj.GetComponent<InventoryItem>();
                
                artifact.OnSelect = () =>
                {
                    currentInventoryItem = artifact;
                    UpdateSelectedItem(data);
                };
                
                artifact.SetElement(data.image);
                artifactsInventory.Add(artifact);
            }
            
            foreach (var jokerId in jokerIds)
            {
                if (!GameSave.Instance.HasItemsId(jokerId.Guid))
                {
                    continue;
                }

                var data = DataRegistry.Instance.JokerRegistry.Get(jokerId);
                   
                var obj = Instantiate(jokerInventoryPrefab, jokerInventoryParent);
                var joker = obj.GetComponent<InventoryItem>();
                
                joker.OnSelect = () =>
                {
                    currentInventoryItem = joker;
                    UpdateSelectedItem(data);
                };
                
                joker.SetElement(data.image);
                jokersInventory.Add(joker);
            }
        }

        private void ClearAllItems()
        {
            foreach (var artifact in artifactsStore)
            {
                Destroy(artifact.gameObject);
            }
            
            foreach (var joker in jokersStore)
            {
                Destroy(joker.gameObject);   
            }
            
            foreach (var artifact in artifactsInventory)
            {
                Destroy(artifact.gameObject);   
            }
            
            foreach (var joker in jokersInventory)
            {
                Destroy(joker.gameObject);   
            }
            
            artifactsStore.Clear();
            jokersStore.Clear();
            artifactsInventory.Clear();
            jokersInventory.Clear();
        }

        private void OnBuyClicked()
        {
            if (!CurrentStoreItem)
                return;
            
            var price = CurrentStoreItem.Price;
            //price
            if (GameSave.Instance.HasEnoughGold(price))
            {
                GameSave.Instance.HasEnoughGold(-price);
            }
            
            //save
            GameSave.Instance.AddPossessItem(CurrentStoreItem.Id);
            
            //apply
            switch (CurrentStoreItem.Type)
            {
                case ItemType.Artifact:
                    var abilityContainer = player.GetComponent<AbilityContainer>();

                    var info = DataRegistry.Instance.ArtifactRegistry.Get(CurrentStoreItem.Id);
                    abilityContainer.currentArtifacts.Add(info);
                    
                    var obj = Instantiate(artifactDisplayPrefab, artifactDisplayParent);
                    var artifact = obj.GetComponent<StoreItem>();
                
                    artifact.OnSelect = () =>
                    {
                        CurrentStoreItem = artifact;
                        UpdateSelectedItem(info);
                    };
                
                    artifact.SetElement(info, CurrentStoreItem.Id, ItemType.Artifact);
                    artifactsStore.Add(artifact);
                    break;
                case ItemType.Joker:
                    break;
            }
            
            //delete
            Destroy(CurrentStoreItem.gameObject);
            CurrentStoreItem = null;
        }

        private List<int> GetUniqueNumbers(int count)
        {
            var uniqueNumbers = new HashSet<int>();
            var random = new System.Random();
            
            while (uniqueNumbers.Count < DisplayNumber)
            {
                int number = random.Next(0, count);
                uniqueNumbers.Add(number);
            }

            return uniqueNumbers.ToList();
        }

        private void UpdateSelectedItem(ItemSO item)
        {
            descriptionText.text = item.description;
        }
    }
}
