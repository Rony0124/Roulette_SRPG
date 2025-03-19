using DG.Tweening;
using TMPro;
using TSoft.Data;
using TSoft.Item;
using TSoft.Managers.TweenSystem;
using UnityEngine;

namespace TSoft.UI.Popup.Inventory
{
    public sealed class ItemInfoPopup : PopupView
    {
        [SerializeField] private TextMeshProUGUI itemTitle;
        [SerializeField] private TextMeshProUGUI itemDescription;
        
        private Vector2 _lastSourcePos;
        private Vector2 _originalPosition;

        protected override void OnActivated()
        {
            base.OnActivated();

            _originalPosition = transform.position;
        }

        public void InitPopup(ItemInfo itemData)
        {
            itemTitle.text = itemData.title;
            itemDescription.text = itemData.description;
        }
        
        public void ShowPanel(Vector2 sourcePos)
        {
            RectTransform rt = transform as RectTransform;
            
            if (sourcePos.x > Screen.width * 0.75f)
            {
                sourcePos.x -= rt.sizeDelta.x;
            }
            
            transform.position = sourcePos;
            
            gameObject.SetActive(true);
            
           // gameObject.transform.SetAsLastSibling();
        }
        
        public void HidePanel()
        {
           // transform.position = _originalPosition;
            
            gameObject.SetActive(false);
        }

    }
}
