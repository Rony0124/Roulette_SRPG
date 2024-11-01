using UnityEngine;

namespace TSoft.InGame.Roulette
{
    public class RouletteSlotOdd : RouletteSlotBase
    {
        [SerializeField] private bool matchOdd;
        
        public override bool CheckMatch(RouletteNumber rouletteNum, ref RouletteController.Bet bet)
        {
            return !(matchOdd ^ rouletteNum.Number % 2 > 1);
        }
    }
}
