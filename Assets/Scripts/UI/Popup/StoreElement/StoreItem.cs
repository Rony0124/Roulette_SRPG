using System;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Popup.StoreElement
{
    public class StoreItem : MonoBehaviour
    {
        public Action OnSelect;
        
        private Button button;
        private Image thumbnail;
        
        private void Start()
        {
            button = GetComponent<Button>();
            thumbnail = GetComponent<Image>();
            
            button.onClick.AddListener(() => OnSelect?.Invoke());
        }

        public void SetElement(Sprite sprite)
        {
            thumbnail.sprite = sprite;
           
        }
    }
}
