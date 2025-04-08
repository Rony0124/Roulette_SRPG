using UnityEngine;

namespace TSoft.InGame
{
    public abstract class ControllerBase : MonoBehaviour
    {
        protected InGameDirector director;
        
        public InGameDirector Director
        {
            get => director;
            set
            {
                OnDirectorChanged();
                director = value;
            }
        }

        protected abstract void OnDirectorChanged();
    }
}
