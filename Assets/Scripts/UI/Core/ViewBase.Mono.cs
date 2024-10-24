using UnityEngine;

namespace TSoft.UI.Core
{
    public abstract partial class ViewBase : MonoBehaviour
    {
        private bool isActive = false;
        private Canvas viewCanvas;

        public bool IsActive {
            get => isActive;
            set {
                isActive = value;

                if (viewCanvas == null) {
                    viewCanvas = GetComponent<Canvas>();
                }

                viewCanvas.enabled = isActive;
            }
        }

        private void Awake() {
            viewCanvas = GetComponent<Canvas>();
            Init();
        }

        private void OnEnable() {
            OnActivated();
        }

        private void OnDisable() {
            OnDeactivated();
        }

        protected abstract void Init();
        protected abstract void OnActivated();
        protected abstract void OnDeactivated();
        
    }
}
