using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame.Roulette
{
    public class RouletteSlotNumber : RouletteSlotBase
    {
        [SerializeField] private int[] matchNumbers;
        
        public override bool CheckMatch(RouletteNumber rouletteNum, ref int bet)
        {
            if (!IsBet())
                return false;
            
            var rulNum = rouletteNum.Number;
            if (matchNumbers.All(checkNumber => checkNumber != rulNum))
                return false;
            
            bet += Wager * 36 / matchNumbers.Length;
            return true;

        }
    }
}
