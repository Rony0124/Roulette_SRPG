using DG.Tweening;
using TMPro;
using TSoft.Data;
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

        public void InitPopup(ItemSO itemData)
        {
            itemTitle.text = itemData.title;
            itemDescription.text = itemData.description;
        }
        
        public void ShowPanel(Vector2 sourcePos)
        {
            gameObject.SetActive(true);

            transform.position = sourcePos;
            
            gameObject.transform.SetAsLastSibling();
        }
        
        public void HidePanel()
        {
            transform.position = _originalPosition;
            
            gameObject.SetActive(false);
        }

    }
}
