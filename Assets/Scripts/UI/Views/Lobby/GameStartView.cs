using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TSoft.UI.Core;
using UI.Page;
using UnityEngine;

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
        
        private int currentPageIndex;
        private UIPage currentPage;
        
        protected override void OnActivated()
        {
            currentPageIndex = 0;
            currentPage = pages[0].page;

            foreach (var page in pages)
            {
                page.page.CurrentView = this;
            }
        }

        protected override void OnDeactivated()
        {
        }

        public void MoveToNextPage()
        {
            if (currentPageIndex + 1 > pages.Count - 1)
            {
                Debug.Log("index out of Pages");
                return;
            }
            
            var page = pages[currentPageIndex + 1].page;
            if (!page) 
                return;
            
            currentPage.gameObject.SetActive(false);
                
            page.gameObject.SetActive(true);
            currentPage = page;
                
            ++currentPageIndex;
        }
        
        public void MoveToPrevPage()
        {
            if (currentPageIndex - 1 < 0)
            {
                Debug.Log("index out of Pages");
                return;
            }
            
            var page = pages[currentPageIndex - 1].page;
            if (!page) 
                return;
            
            currentPage.gameObject.SetActive(false);
            
            page.gameObject.SetActive(true);
            currentPage = page;
                
            --currentPageIndex;
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

            currentPageIndex = (int)pageId;
        }
        
    }
}
