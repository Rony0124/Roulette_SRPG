using System;
using TSoft.Data;
using TSoft.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class ItemIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Action<ItemInfo, Vector2> onPointerEnter;
        public Action onPointerExit;
        
        [SerializeField] protected Image itemIcon;
        [SerializeField] protected GameObject disableImage;
        
        protected ItemInfo currentItem = default;

        public void SetItemIcon(ItemInfo item)
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
            
            onPointerEnter?.Invoke(currentItem, transform.position);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke();
        }

        private void OnDestroy()
        {
            onPointerEnter = null;
            onPointerExit = null;
        }
    }
}
