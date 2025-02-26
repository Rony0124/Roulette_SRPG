using TSoft.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.UI.Popup.Inventory
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public CanvasGroup canvasGroup;
        
        [HideInInspector]
        public Transform parentAfterDrag;

        public bool IsDragging() => parentAfterDrag != null;

        private Transform rootTransform => PopupContainer.Instance.GetCurrentPopup()?.transform;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent;
            if (rootTransform == null)
            {
                transform.SetParent(transform.root);    
            }
            else
            {
                transform.SetParent(rootTransform);
            }
            
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentAfterDrag);
            transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            parentAfterDrag = null;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
