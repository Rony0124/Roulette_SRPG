using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TSoft.UI.Core;
using UI.Page;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Views.Lobby
{
    public class GameStartView : ViewBase
    {
        public enum Page
        {
            GameStartPage = 0,
            LobbyPage = 1,
        }
        
        [Serializable]
        public struct PageInfo
        {
            public Page pageId;
            public UIPage page;
        }
        
        [TableList]
        [SerializeField] private List<PageInfo> pages;
        
        [SerializeField] private UnityEvent onPageChangedFeedback; 
        
        private int currentPageIndex;
        private UIPage currentPage;

        private int CurrentPageIndex
        {
            get => currentPageIndex;
            set
            {
                if (currentPageIndex == value || value < 0 || value >= pages.Count)
                    return;
                
                OnCurrentPageChanged();
                currentPageIndex = value;
            }
        }
        
        protected override void OnActivated()
        {
            CurrentPageIndex = 0;
            currentPage = pages[0].page;

            foreach (var page in pages)
            {
                page.page.CurrentView = this;
            }
        }

        protected override void OnDeactivated()
        {
        }

        private void OnCurrentPageChanged()
        {
            onPageChangedFeedback?.Invoke();
        }

        public void MoveToNextPage()
        {
            if (CurrentPageIndex + 1 > pages.Count - 1)
            {
                Debug.Log("index out of Pages");
                return;
            }
            
            var page = pages[CurrentPageIndex + 1].page;
            if (!page) 
                return;
            
            currentPage.gameObject.SetActive(false);
                
            page.gameObject.SetActive(true);
            currentPage = page;
                
            ++CurrentPageIndex;
        }
        
        public void MoveToPrevPage()
        {
            if (CurrentPageIndex - 1 < 0)
            {
                Debug.Log("index out of Pages");
                return;
            }
            
            var page = pages[CurrentPageIndex - 1].page;
            if (!page) 
                return;
            
            currentPage.gameObject.SetActive(false);
            
            page.gameObject.SetActive(true);
            currentPage = page;
                
            --CurrentPageIndex;
        }

        public void MoveToDesignatedPage(Page pageId)
        {
            if ((int)pageId > pages.Count - 1)
            {
                Debug.Log("index out of Pages");
                return;
            }
            
            var page = pages[(int)pageId].page;
            if (!page) 
                return;
            
            currentPage.gameObject.SetActive(false);
                
            page.gameObject.SetActive(true);
            currentPage = page;

            CurrentPageIndex = (int)pageId;
        }
        
    }
}
