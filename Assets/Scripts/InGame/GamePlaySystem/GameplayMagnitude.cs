using System;
using HF.InGame;
using InGame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    public enum MagnitudeType
    {
        None,
        Random,
        Curve
    }
    
    [Serializable]
    public struct GameplayMagnitude
    {
        public MagnitudeType magnitudeType;
        
        [ShowIf("magnitudeType", MagnitudeType.None)]
        public float magnitude;
        
        [ShowIf("magnitudeType", MagnitudeType.Random)]
        public RandomMagnitude magnitudeRandom;
        
        [ShowIf("magnitudeType", MagnitudeType.Curve)]
        [SerializeReference]
        public GameplayMagnitudeCalculator magnitudeCurve;
    }

    [Serializable]
    public struct RandomMagnitude
    {
        public float minValue;
        public float maxValue;
        
        public float RandomValue => UnityEngine.Random.Range(minValue, maxValue);
    }

    public abstract class GameplayMagnitudeCalculator
    {
        public AnimationCurve magnitudeCurve;
        
        public abstract float CalculateCurve(InGameDirector director);
    }

    public class ComboCalculator : GameplayMagnitudeCalculator
    {
        public override float CalculateCurve(InGameDirector director)
        {
            return 0;
            /*var player = director.Player;
            var combo = player.previousPatterns.Count;

            return magnitudeCurve.Evaluate(combo);*/
        }
    }
}
