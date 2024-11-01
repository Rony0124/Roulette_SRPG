using UnityEngine;

namespace TSoft.Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T instance;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType(typeof(T)) as T;

                    if (instance == null) {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();

                        singletonObject.name = typeof(T).Name;
                    }
                }

                return instance;
            }
        }
    }
}
