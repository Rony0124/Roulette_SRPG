using UnityEngine;

namespace TSoft.InGame
{
    public abstract class DirectorBase : MonoBehaviour
    {
        private void Awake()
        {
            InitOnAwake();
        }

        protected virtual void InitOnAwake()
        {
            GameContext.Instance.OnDirectorChanged += OnDirectorChanged;
            
            GameContext.Instance.CurrentDirector = this;
        }
        
        //director의 초기화는 여기서 해준다
        protected abstract void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue);
    }
}

