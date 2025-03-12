using TSoft.UI.Core;
using UnityEngine;

namespace TSoft.UI.Views
{
    public class ViewModel : MonoBehaviour
    {
        protected ViewBase view;
        protected Model model;
        
        public virtual void Awake()
        {
            view = GetComponent<ViewBase>();
            model = GetComponent<Model>();
        }
    }
}
