using System.Linq;
using UnityEngine;

namespace TSoft.Utils
{
    public static class UIUtil
    {
        public static bool TryFindChild(GameObject go, out GameObject obj, string name = null, bool recursive = false)
        {
            obj = null;
            var transform = FindChild<Transform>(go, name, recursive);

            if (!transform)
                return false;
            
            obj = transform.gameObject;
            return true;
        }
        
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            var transform = FindChild<Transform>(go, name, recursive);
            
            return transform ? null : transform.gameObject;
        }
        
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
        {
            if (!go)
                return null;

            if(recursive == false)
            {
                for (var i = 0; i < go.transform.childCount; i++)
                {
                    var transform = go.transform.GetChild(i);
                    if (!string.IsNullOrEmpty(name) && transform.name != name) continue;
                    
                    var component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
            else
            {
                return go.GetComponentsInChildren<T>().FirstOrDefault(component => string.IsNullOrEmpty(name) || component.name == name);
            }
            return null;
        }
        
        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            
            if (component == null) 
                component = go.AddComponent<T>();
            
            return component;
        }
    }
}
