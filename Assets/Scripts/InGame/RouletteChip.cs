using System.ComponentModel.Design;
using TSoft.Core;
using TSoft.InGame.Roulette;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace TSoft.InGame
{
    public class RouletteChip : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject DraggedIcon;
        [HideInInspector] public Transform startParent;
        public Transform onDragParent;

        public int wager;
        
        public UnityEvent<RouletteChip> OnDragBegin;
        public UnityEvent<RouletteChip> OnDragEnd;
        
        private Vector3 startPosition;
 
        public void OnBeginDrag(PointerEventData eventData)
        {
            DraggedIcon = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            
            transform.SetParent(onDragParent);
            
            OnDragBegin?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DraggedIcon = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            if (transform.parent == onDragParent)
            {
                Destroy(gameObject);
            }
        }
    }
}
