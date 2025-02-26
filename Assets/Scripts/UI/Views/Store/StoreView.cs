using System;
using System.Collections.Generic;
using System.Linq;
using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Core;
using TSoft.UI.Popup.StoreElement;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
            BackButton
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

            Get<Button>((int)StoreButton.BackButton).onClick.AddListener(OnExitClicked);
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
            List<Guid> GetAvailableItems()
            {
                var guids = new List<Guid>();
                foreach (var itemId in registry.Ids)
                {
                    if (GameSave.Instance.HasItemsId(itemId.Guid))
                        continue;
                    
                    guids.Add(itemId.Guid);
                }
                
                return guids;
            }
            
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
