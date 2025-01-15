using System;
using Sirenix.OdinInspector;

namespace TSoft.InGame.GamePlaySystem
{
    [Serializable]
    public class GameplayDuration
    {
        public enum PolicyType
        {
            Instant, Infinite, HasDuration
        }
        
        public PolicyType policy;
        
        [ShowIf("policy", PolicyType.HasDuration)]
        public float magnitude;
    }
}
