using System;
using UnityEngine;

namespace TSoft.InGame.Roulette
{
    [Serializable]
    public struct RouletteNumber
    {
        public int Number;
        //red이면 true, black이면 0
        public bool Color;
        public SectionType SectionType;
    }
    
    public abstract class RouletteSlotBase : MonoBehaviour, IRouletteSlotHandler
    {
        public int Wager;

        public virtual bool IsBet() => Wager > 0;

        public abstract bool CheckMatch(RouletteNumber rouletteNum, ref RouletteController.Bet bet);
    }
}
