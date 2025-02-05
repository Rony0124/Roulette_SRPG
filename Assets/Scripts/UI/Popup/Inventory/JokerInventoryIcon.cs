using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class JokerInventoryIcon : ItemIcon
    {
        public Action<JokerInventoryIcon> onItemClicked;
        public Action<JokerInventoryIcon> onItemReleased;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnPointerUp(PointerEventData eventData)
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
                    Debug.Log("아이템 스펙 설명");
                }
                
                onItemClicked?.Invoke(this);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            onItemReleased?.Invoke(this);
        }
    }
}
