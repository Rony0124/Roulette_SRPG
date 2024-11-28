using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace TSoft.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] private bool dontDestroyOnLoad;
        
        private static T instance;
        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType(typeof(T)) as T;

                    if (instance == null) {
                        var go = new GameObject();
                        instance = go.AddComponent<T>();
                        
                        go.name = typeof(T).Name;
                    }
                    
                    if (instance.dontDestroyOnLoad)
                    {
                        if (instance.transform.root != null)
                        {
                            instance.transform.SetParent(null);
                        }
                
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
                
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Init();
        }

        protected virtual void Init() { }
    }
}
