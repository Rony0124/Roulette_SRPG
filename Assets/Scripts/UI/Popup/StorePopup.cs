using System.Collections.Generic;
using System.Linq;
using TMPro;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.UI.Popup.StoreElement;
using UnityEditor;
using UnityEngine;

namespace TSoft.UI.Popup
{
    public class StorePopup : PopupView
    {
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

        [SerializeField] 
        private GameObject artifactDisplayPrefab;
        [SerializeField] 
        private GameObject jokerDisplayPrefab;
        
        [SerializeField] 
        private GameObject artifactInventoryPrefab;
        [SerializeField] 
        private GameObject jokerInventoryPrefab;
        
        //ui
        private Transform artifactDisplayParent;
        private Transform jokerDisplayParent;
        private Transform artifactInventoryParent;
        private Transform jokerInventoryParent;
        private TextMeshProUGUI descriptionText;
        
        //item
        private List<StoreItem> artifactsStore = new();
        private List<StoreItem> jokersStore = new();
        private List<InventoryItem> artifactsInventory = new();
        private List<InventoryItem> jokersInventory = new();
            
        private const int DisplayNumber = 3;
        
        private void Awake()
        {
            Bind<Transform>(typeof(DisplayParent));
            Bind<Transform>(typeof(InventoryParent));
            Bind<TextMeshPro>(typeof(Text));
            

            artifactDisplayParent = Get<Transform>((int)DisplayParent.Artifacts_Display);
            jokerDisplayParent = Get<Transform>((int)DisplayParent.Jokers_Display);
            artifactInventoryParent = Get<Transform>((int)InventoryParent.Artifacts_Inventory);
            jokerInventoryParent = Get<Transform>((int)InventoryParent.Jokers_Inventory);
            descriptionText = Get<TextMeshProUGUI>((int)Text.DescriptionText);
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
                    UpdateSelectedItem(info);
                };
                
                artifact.SetElement(info.image);
                artifactsStore.Add(artifact);
            }
            
            for (int i = 0; i < DisplayNumber; i++)
            {
                if (!DataRegistry.Instance.JokerRegistry.TryGetDataByIndex(jokerNums[i], out var info))
                {
                    continue;
                }
                
                var obj = Instantiate(jokerDisplayPrefab, jokerDisplayParent);
                var joker = obj.GetComponent<StoreItem>();
                joker.OnSelect = () =>
                {
                    UpdateSelectedItem(info);
                };
                
                joker.SetElement(info.image);
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
