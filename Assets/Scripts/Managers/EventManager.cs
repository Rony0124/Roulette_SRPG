using System;
using System.Collections.Generic;
using TSoft.Core;
using TSoft.Data;
using TSoft.InGame.GamePlaySystem;
using UnityEngine.Events;

namespace TSoft.Managers
{
    public class EventManager : Singleton<EventManager>
    {
        public class DamageAdderEvent
        {
            private UnityEvent<AppliedModifier> dmgEvent;

            public void AddEvent(UnityAction<AppliedModifier> action)
            {
                if (dmgEvent == null)
                {
                    var newEvent = new UnityEvent<AppliedModifier>();
                    newEvent.AddListener(action);
                    dmgEvent = newEvent;
                }
                else
                {
                    dmgEvent.AddListener(action);
                }
            }

            public void Raise(AppliedModifier modifier)
            {
                dmgEvent?.Invoke(modifier);
            }
        }
        
        public DamageAdderEvent DmgAdderEvent = new();
    }
}
