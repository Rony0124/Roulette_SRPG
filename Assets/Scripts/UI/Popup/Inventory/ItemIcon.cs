using TSoft.Data;
using TSoft.Managers;
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
        protected bool enableItemPointerAction = true;

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

            if (!enableItemPointerAction)
                return;

            if (eventData.button == PointerEventData.InputButton.Left && eventData.pointerPress == gameObject)
            {
                Vector2 pressPosition = eventData.pressPosition;

                if (Vector2.Distance(pressPosition, eventData.position) < 10)
                {
                    var infoPanel = PopupContainer.Instance.GetPopupUI(PopupContainer.PopupType.ItemInfo) as ItemInfoPopup;
                    if (infoPanel == null) 
                        return;
                    
                    infoPanel.InitPopup(currentItem);
                    infoPanel.ShowPanel(transform.position);
                }
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            var infoPanel = PopupContainer.Instance.GetPopupUI(PopupContainer.PopupType.ItemInfo) as ItemInfoPopup;
            if (infoPanel == null) 
                return;
            
            infoPanel.HidePanel();
        }
    }
}
