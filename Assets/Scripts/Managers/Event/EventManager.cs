using System.Collections.Generic;
using HF.Core;
using TSoft.Core;
using TSoft.Data;
using UnityEngine.Events;

namespace TSoft.Managers.Event
{
    public class EventManager : Singleton<EventManager>
    {
        public readonly DamageAdderEvent DmgAdderEvent = new();
        public readonly GameEffectEvent GameEvent = new();

       
    }
}
