using UnityEngine;

namespace HF.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T> ();
                    
                    if (instance != null)
                        return instance;
                    
                    var obj = new GameObject
                    {
                        name = typeof(T).Name
                    };
                    
                    instance = obj.AddComponent<T> ();
                }
                return instance;
            }
        }

        private void OnApplicationQuit()
        {
            Time.timeScale = 0;
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
    }
}
