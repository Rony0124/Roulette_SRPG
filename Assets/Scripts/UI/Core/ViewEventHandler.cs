using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.UI.Core
{
    public class ViewEventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
    {
        public Action<PointerEventData> OnClickHandler = null;
        public Action<PointerEventData> OnDragHandler = null;

        public void OnPointerClick(PointerEventData eventData) // 클릭 이벤트 오버라이딩
        {
            if (OnClickHandler != null)
                OnClickHandler.Invoke(eventData); // 클릭와 관련된 액션 실행
        }

        public void OnDrag(PointerEventData eventData) // 드래그 이벤트 오버라이딩
        {
            if (OnDragHandler != null)
                OnDragHandler.Invoke(eventData); // 드래그와 관련된 액션 실행
        }
    }
}
