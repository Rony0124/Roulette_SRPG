using System;
using Unity.VisualScripting;
using UnityEngine;

namespace TSoft.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static object @lock = new();
        
        private static T instance;
        public static T Instance {
            get {
                if (instance == null && Time.timeScale != 0) {
                    lock (@lock)
                    {
                        instance = FindObjectOfType(typeof(T)) as T;

                        if (instance == null) {
                            var singletonObject = new GameObject();
                            instance = singletonObject.AddComponent<T>();

                            singletonObject.name = typeof(T).Name;
                        }
                    }
                }
                
                return instance;
            }
        }
        
        protected virtual void Awake ()
        {
            InitializeSingleton();		
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            instance = this as T;
        }

        private void OnApplicationQuit()
        {
            Time.timeScale = 0;
        }
    }
}
