using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TSoft.Core
{
    public class SingletonScriptable<T>  : ScriptableObject where T : SingletonScriptable<T> {
        [LabelText("DontDestroyOnLoad")]
        [SerializeField]
        private bool isNotDestroyed = false;
        public bool IsNotDestroyed => isNotDestroyed;

        private static bool _isInstantiated = false;
        public static bool IsInstantiated => _isInstantiated;

        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    T[] assets = Resources.LoadAll<T>("Managers");
                    if (assets == null || assets.Length < 1) {
                        Debug.LogError($"There is not {typeof(T).Name} in Resources/Managers");
                    } else if (assets.Length > 1) {
                        Debug.LogWarning($"There is more than one {typeof(T).Name} in Resources/Managers");
                    } else {
                        _instance = Instantiate(assets.First());
                        _isInstantiated = true;
                    }

                    if (_instance.IsNotDestroyed) {
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            Initialize();
        }

        public virtual void Initialize() { }
       
    }
}
