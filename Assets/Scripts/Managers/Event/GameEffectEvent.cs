using System.Collections.Generic;
using TSoft.Data;
using UnityEngine.Events;

namespace TSoft.Managers.Event
{
    public sealed class GameEffectEvent
    {
        private Dictionary<RegistryId, UnityEvent> gameEvents = new();
        
        public void Subscribe(RegistryId id, UnityAction action)
        {
            if (gameEvents.TryGetValue(id, out var unityEvent))
            {
                unityEvent.AddListener(action);
            }
            else
            {
                unityEvent = new UnityEvent();
                unityEvent.AddListener(action);
                gameEvents.Add(id, unityEvent);
            }
        }
            
        public void Unsubscribe(RegistryId id, UnityAction action)
        {
            if (gameEvents.TryGetValue(id, out var unityEvent))
            {
                unityEvent.RemoveListener(action);
            }
        }

        public void Raise(RegistryId id)
        {
            if (gameEvents.TryGetValue(id, out var unityEvent))
            {
                unityEvent?.Invoke();
            }
        }
    }
}
