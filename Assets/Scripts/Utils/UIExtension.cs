using System;
using HF.Utils;
using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TSoft.Utils
{
    public static class UIExtension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component 
        {
            return UIUtil.GetOrAddComponent<T>(go);
        }

        public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click) 
        {
            ViewBase.BindEvent(go, action, type);
        }
    }
}
