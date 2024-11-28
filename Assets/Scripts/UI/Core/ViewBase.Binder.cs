using System;
using System.Collections.Generic;
using TMPro;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TSoft.UI.Core
{
    public abstract partial class ViewBase
    {
        protected Dictionary<Type, Object[]> Objects = new ();

        protected void Bind<T>(Type type) where T : Object
        {
            var names = Enum.GetNames(type);
            var objects = new Object[names.Length];
            Objects.Add(typeof(T), objects);

            for (var i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = UIUtil.FindChild(gameObject, names[i], true);
                else
                    objects[i] = UIUtil.FindChild<T>(gameObject, names[i], true);

                if (objects[i] == null)
                    Debug.Log($"Failed to bind({names[i]})");
            }
        }
        
        public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            var evt = UIUtil.GetOrAddComponent<ViewEventHandler>(go);

            switch (type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case Define.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
            }
        }
        
        protected T Get<T>(int idx) where T : Object
        {
            if (!Objects.TryGetValue(typeof(T), out var objects))
                return null;

            return objects[idx] as T;
        }
    }
}