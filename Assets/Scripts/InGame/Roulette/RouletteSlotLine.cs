using UnityEngine;

namespace TSoft.InGame.Roulette
{
    public class RouletteSlotLine : RouletteSlotBase
    {
        [SerializeField] private int matchNumber;
        
        public override bool CheckMatch(RouletteNumber rouletteNum, ref int bet)
        {
            var rulNum = rouletteNum.Number;
            if (rulNum % 3 != matchNumber) 
                return false;
            
            bet += Wager * 3;
            return true;

        }
    }
}
