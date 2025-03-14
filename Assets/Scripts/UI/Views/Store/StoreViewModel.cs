using System;
using System.Collections.Generic;
using System.Linq;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Popup.StoreElement;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace TSoft.UI.Views.Store
{
    public class StoreViewModel : ViewModelBase
    {
        private StoreView View => view as StoreView;
        
        [SerializeField] 
        private GameObject artifactDisplayPrefab;
        [SerializeField] 
        private GameObject jokerDisplayPrefab;
        [SerializeField] 
        private GameObject skillDisplayPrefab;
        
        private StoreItem currentStoreItem;
        private List<StoreItem> items = new();
        
        private const int MaxDisplayNumber = 3;

        private void Start()
        {
            View.backButton.onClick.AddListener(OnExitClicked);
            
            CreateDisplayItems();
        }
        
        private void CreateDisplayItems()
        {
            CreateDisplay(
                DataRegistry.Instance.JokerRegistry,
                jokerDisplayPrefab,
                View.jokerDisplayParent
            );

            CreateDisplay(
                DataRegistry.Instance.ArtifactRegistry,
                artifactDisplayPrefab,
                View.artifactDisplayParent
            );

            CreateDisplay(
                DataRegistry.Instance.SkillRegistry,
                skillDisplayPrefab,
                View.skillDisplayParent
            );
        }

        private void CreateDisplay<T>(Registry<T> registry, GameObject prefab, Transform parent)
            where T : ItemSO
        {
            var availableItems = GetAvailableItems();
            var uniqueNumbers = GetUniqueNumbers(availableItems.Count);

            if (uniqueNumbers.Count == 0)
            {
                Debug.Log("[Store] No available items to display.");
                return;
            }

            foreach (var uniqueNumber in uniqueNumbers)
            {
                var item = registry.Get(availableItems[uniqueNumber]);
                    
                var obj = Instantiate(prefab, parent);
                var storeItem = obj.GetComponent<StoreItem>();
                var info = item;

                storeItem.onSelect = () =>
                {
                    OnSelect(storeItem);
                };

                storeItem.onBuyClicked = OnBuyClicked;

                storeItem.SetElement(info);
                items.Add(storeItem);
            }

            return;

            List<int> GetUniqueNumbers(int maxNumber)
            {
                if(maxNumber == 0)
                    return new List<int>();
            
                var uniqueNumbers = new HashSet<int>();
                var min = Mathf.Min(maxNumber, MaxDisplayNumber);
            
                while (uniqueNumbers.Count < min)
                {
                    int number = Random.Range(0, maxNumber);
                    
                    uniqueNumbers.Add(number);
                }

                return uniqueNumbers.ToList();
            }

            List<Guid> GetAvailableItems()
            {
                var guids = new List<Guid>();
                foreach (var kvp in registry.assetGuidLookup)
                {
                    if (GameSave.Instance.HasItemsId(kvp.Key))
                        continue;
                    
                    guids.Add(kvp.Key);
                }
                
                return guids;
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
            }
            else
            {
                SceneManager.LoadScene(Define.StageMap);    
            }
        }
    }
}
