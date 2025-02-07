using System;
using Sirenix.OdinInspector;

namespace TSoft.InGame.GamePlaySystem
{
    [Serializable]
    public class GameplayDuration
    {
        public enum PolicyType
        {
            Passive,
            OnRoundBegin,
            OnRoundFinished,
            OnTurnBegin,
            OnTurnFinished
        }
        
        public PolicyType policy;
    }
}
