using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TSoft.Core;
using UnityEngine;

namespace TSoft.Managers
{
    public class PopupManager : Singleton<PopupManager>
    {
        public enum PopupType
        {
            GameOver,
            Win
        }
        
        [Serializable]
        public class Popup
        {
            public PopupType type;
            public GameObject popupObj;
        }

        [TableList] 
        public List<Popup> popups;

        private GameObject currentOpenPopup;

        public void OpenPopup(PopupType type)
        {
            var popup = popups.Find(Popup => Popup.type == type).popupObj;
            if (popup is not null)
            {
                popup.SetActive(true);
            }

            currentOpenPopup = popup;
        }
        
        public void ClosePopup(PopupType type)
        {
            var popup = popups.Find(Popup => Popup.type == type).popupObj;
            if (popup is not null)
            {
                popup.SetActive(false);
            }

            if (currentOpenPopup == popup)
            {
                currentOpenPopup = null;
            }
        }

        public void ClosePrevPopup()
        {
            if (currentOpenPopup != null && currentOpenPopup.activeSelf)
            {
                currentOpenPopup.SetActive(false);
                currentOpenPopup = null;    
            }
        }
    }
}
