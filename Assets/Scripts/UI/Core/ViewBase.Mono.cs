using UnityEngine;

namespace TSoft.UI.Core
{
    public abstract partial class ViewBase : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;
        

        private void OnEnable() {
            OnActivated();
        }

        private void OnDisable() {
            OnDeactivated();
        }
        
        protected abstract void OnActivated();
        protected abstract void OnDeactivated();
        
    }
}
