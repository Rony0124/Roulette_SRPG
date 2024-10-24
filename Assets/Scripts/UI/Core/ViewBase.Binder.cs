using System;
using System.Collections.Generic;
using TMPro;
using TSoft.Utils;
using UnityEngine;
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
        
        protected T Get<T>(int idx) where T : Object
        {
            if (!Objects.TryGetValue(typeof(T), out var objects))
                return null;

            return objects[idx] as T;
        }

        protected GameObject GetObject(int idx)  => Get<GameObject>(idx); 
        protected TextMeshProUGUI GetText(int idx) => Get<TextMeshProUGUI>(idx);
        protected Button GetButton(int idx) => Get<Button>(idx);
        protected Image GetImage(int idx) => Get<Image>(idx);
        protected Slider GetSlider(int idx) => Get<Slider>(idx);
    }
}