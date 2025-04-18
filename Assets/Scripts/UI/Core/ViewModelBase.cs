using TSoft.UI.Core;
using UnityEngine;

namespace TSoft.UI.Views
{
    public class ViewModelBase : MonoBehaviour
    {
        protected ViewBase view;
        protected ModelBase model;
        
        public virtual void Awake()
        {
            view = GetComponent<ViewBase>();
            model = GetComponent<ModelBase>();
        }
    }
}
