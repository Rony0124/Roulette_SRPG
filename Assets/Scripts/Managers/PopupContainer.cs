using System;
using System.Collections.Generic;
using HF.Utils;
using Sirenix.OdinInspector;
using TSoft.Core;
using TSoft.UI.Popup;
using UnityEngine;
using Utils;

namespace TSoft.Managers
{
    public class PopupContainer : Singleton<PopupContainer>
    {
        public enum PopupType
        {
            GameOver,
            Win,
            Skill,
            Joker,
            Artifact
        }
        
        [Serializable]
        public class Popup
        {
            public PopupType type;
            public GameObject popupObj;
        }

        [TableList] 
        public List<Popup> popups;
        
        private int order = 10; 
        
        UniqueStack<PopupView> popupStack = new ();
        
        public void SetCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = UIUtil.GetOrAddComponent<Canvas>(go);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            if (sort)
            {
                canvas.sortingOrder = order;
                order++;
            }
            else // sort 요청을 안 한 경우는 Popup UI와 관련이 없는 일반 UI라는 것
            {
                canvas.sortingOrder = 0;
            }    
        }
        
        public void ShowPopupUI(PopupType type)
        {
            ClosePopupUI();
            
            GameObject go = popups.Find(Popup => Popup.type == type).popupObj;
            go.SetActive(true);

            SetCanvas(go);
            
            var popup = UIUtil.GetOrAddComponent<PopupView>(go);
            popupStack.Push(popup);
        }
        
        public PopupView GetPopupUI(PopupType type)
        {
            ClosePopupUI();
            
            GameObject go = popups.Find(Popup => Popup.type == type).popupObj;

            SetCanvas(go);
            
            var popup = UIUtil.GetOrAddComponent<PopupView>(go);
            return popup;
        }
        
        public void ClosePopupUI(PopupView popup)
        {
            if (popupStack.Count == 0)
                return;

            if(popupStack.Peek() != popup)
            {
                Debug.Log("Close Popup Failed!");
                return;
            }

            ClosePopupUI();
        }

        public void ClosePopupUI()
        {
            if (popupStack.Count == 0)
                return;

            PopupView popup = popupStack.Pop();
            popup.gameObject.SetActive(false);

            order--;
        }

        public PopupView GetCurrentPopup()
        {
            return popupStack.Peek();
        }

        public void CloseAllPopupUI()
        {
            while (popupStack.Count > 0)
                ClosePopupUI();
        }
        
    }
}
