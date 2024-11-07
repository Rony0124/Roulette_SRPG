using UnityEngine;

namespace TSoft.InGame.Roulette
{
    public class RouletteSlotColor : RouletteSlotBase
    {
        [SerializeField] private bool matchColor;
        
        public override bool CheckMatch(RouletteNumber rouletteNum, ref int bet)
        {
            if (matchColor != rouletteNum.Color) 
                return false;
            
            bet += Wager * 2;
            return true;

        }
    }
}
