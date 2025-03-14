using UnityEngine.Events;

namespace TSoft.Managers.Event
{
    public abstract class GameEvent<T>
    {
        protected UnityEvent<T> gameEvent;
        
        public virtual void AddEvent(UnityAction<T> action)
        {
            if (gameEvent == null)
            {
                var newEvent = new UnityEvent<T>();
                newEvent.AddListener(action);
                gameEvent = newEvent;
            }
            else
            {
                gameEvent.AddListener(action);
            }
        }
            
        public virtual void RemoveListener(UnityAction<T> action)
        {
            gameEvent.RemoveListener(action);
        }
            
        public virtual void RemoveAllListener()
        {
            gameEvent.RemoveAllListeners();
        }

        public void Raise(T value)
        {
            gameEvent?.Invoke(value);
        }
    }
}
