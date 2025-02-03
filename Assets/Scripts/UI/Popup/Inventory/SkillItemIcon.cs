using TSoft.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillItemIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private GameObject disableImage;
        
        private ItemSO currentItem = default;
        private bool enableItemPointerAction = true;

        public void SetItemIcon(ItemSO item)
        {
            currentItem = item;

            itemIcon.sprite = item.image;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
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
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
