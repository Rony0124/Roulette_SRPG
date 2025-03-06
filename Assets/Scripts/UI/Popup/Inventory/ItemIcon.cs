using TSoft.Data;
using TSoft.Managers;
using TSoft.UI.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class ItemIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Image itemIcon;
        [SerializeField] protected GameObject disableImage;
        
        protected ItemSO currentItem = default;

        public void SetItemIcon(ItemSO item)
        {
            currentItem = item;

            itemIcon.sprite = item.image;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.dragging)
                return;

            if (currentItem == null)
                return;
            
            var infoPanel = BootstrapView.Instance.itemInfo;
            if (infoPanel == null) 
                return;
            
            infoPanel.InitPopup(currentItem);
            infoPanel.ShowPanel(transform.position);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            var infoPanel = BootstrapView.Instance.itemInfo;
            if (infoPanel == null) 
                return;
            
            infoPanel.HidePanel();
        }
    }
}
