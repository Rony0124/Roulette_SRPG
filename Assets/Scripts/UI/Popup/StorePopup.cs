using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using TSoft.Data;
using TSoft.Data.Card;
using TSoft.Data.Registry;
using TSoft.InGame;
using TSoft.InGame.Player;
using TSoft.UI.Popup.StoreElement;
using UnityEngine;
using UnityEngine.Rendering;
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
        
        private enum TransformParent
        {
            Artifacts_Display,
            Jokers_Display,
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
            SellButton,
            ExitButton
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
        private PlayerController player;
        
        //ui
        private Transform artifactDisplayParent;
        private Transform jokerDisplayParent;
        private Transform artifactInventoryParent;
        private Transform jokerInventoryParent;
        private TextMeshProUGUI descriptionText;
        private Button buyButton;
        
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
                buyButton.enabled = value != null;
                currentStoreItem = value;
            }
        }
        private InventoryItem currentInventoryItem;
        
        private const int DisplayNumber = 3;
        
        private void Awake()
        {
            Bind<Transform>(typeof(TransformParent));
            Bind<TextMeshProUGUI>(typeof(Text));
            Bind<Button>(typeof(StoreButton));
            
            artifactDisplayParent = Get<Transform>((int)TransformParent.Artifacts_Display);
            jokerDisplayParent = Get<Transform>((int)TransformParent.Jokers_Display);
            artifactInventoryParent = Get<Transform>((int)TransformParent.Artifacts_Inventory);
            jokerInventoryParent = Get<Transform>((int)TransformParent.Jokers_Inventory);
            descriptionText = Get<TextMeshProUGUI>((int)Text.DescriptionText);
            buyButton = Get<Button>((int)StoreButton.BuyButton);
            Get<Button>((int)StoreButton.ExitButton).onClick.AddListener(OnExitClicked);
            
            buyButton.onClick.AddListener(OnBuyClicked);
        }

        protected override void OnActivated()
        {
            CreateDisplayItems();
            CreateInventoryItems();

            player.AbilityContainer.currentArtifacts.ListChanged += OnArtifactsChanged;
        }
        
        protected override void OnDeactivated()
        {
            ClearAllItems();
            
            player.AbilityContainer.currentArtifacts.ListChanged -= OnArtifactsChanged;
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
            
            /*for (int i = 0; i < DisplayNumber; i++)
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
            }*/
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

                CreateArtifactInventory(data);
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

        private void CreateArtifactInventory(ArtifactSO data)
        {
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
            if (!GameSave.Instance.HasEnoughGold(price))
                return;
            
            GameSave.Instance.HasEnoughGold(-price);
            
            //save
            GameSave.Instance.AddPossessItem(CurrentStoreItem.Id);
            
            //apply
            switch (CurrentStoreItem.Type)
            {
                case ItemType.Artifact:
                    var abilityContainer = player.AbilityContainer;

                    var artifactInfo = DataRegistry.Instance.ArtifactRegistry.Get(CurrentStoreItem.Id);
                    abilityContainer.currentArtifacts.Add(artifactInfo);
                    
                    /*var obj = Instantiate(artifactDisplayPrefab, artifactDisplayParent);
                    var artifact = obj.GetComponent<StoreItem>();
                
                    artifact.OnSelect = () =>
                    {
                        CurrentStoreItem = artifact;
                        UpdateSelectedItem(artifactInfo);
                    };
                
                    artifact.SetElement(artifactInfo, CurrentStoreItem.Id, ItemType.Artifact);
                    artifactsStore.Add(artifact);*/
                    break;
                case ItemType.Joker:
                    var jokerInfo = DataRegistry.Instance.JokerRegistry.Get(CurrentStoreItem.Id);
                    player.AddJoker(jokerInfo);
                    break;
            }
            
            //delete
            Destroy(CurrentStoreItem.gameObject);
            CurrentStoreItem = null;
        }

        private void OnExitClicked()
        {
            var director = GameContext.Instance.CurrentDirector as InGameDirector;
            if (director != null) 
                director.GameFinishSuccess();
        }
        
        private void OnArtifactsChanged(object sender, ListChangedEventArgs args)
        {
            switch (args.ListChangedType)
            {
                case ListChangedType.ItemAdded :
                    CreateArtifactInventory(player.AbilityContainer.currentArtifacts[args.NewIndex]);
                    break;
                case ListChangedType.ItemDeleted :
                    break;
                case ListChangedType.ItemChanged :
                    break;
                    
            }
        }

        private List<int> GetUniqueNumbers(int count)
        {
            if(count == 0)
                return new List<int>();
            
            var uniqueNumbers = new HashSet<int>();
            var random = new System.Random();
            var min = Mathf.Min(count, DisplayNumber);
            
            while (uniqueNumbers.Count < min)
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
