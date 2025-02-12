using System;
using Sirenix.OdinInspector;

namespace TSoft.InGame.GamePlaySystem
{
    public enum MagnitudeType
    {
        None,
        Random
    }
    
    [Serializable]
    public struct GameplayMagnitude
    {
        public MagnitudeType magnitudeType;
        
        [ShowIf("magnitudeType", MagnitudeType.None)]
        public float magnitude;
        
        [ShowIf("magnitudeType", MagnitudeType.Random)]
        public RandomMagnitude magnitudeRandom;
    }

    [Serializable]
    public struct RandomMagnitude
    {
        public float minValue;
        public float maxValue;
        
        public float RandomValue => UnityEngine.Random.Range(minValue, maxValue);
    }
    
    
}
