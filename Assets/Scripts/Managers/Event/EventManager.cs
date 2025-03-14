using TSoft.Core;

namespace TSoft.Managers.Event
{
    public class EventManager : Singleton<EventManager>
    {
        public readonly DamageAdderEvent DmgAdderEvent = new();
    }
}
