using UnityEngine;

namespace TSoft.InGame.Roulette
{
    public class RouletteSlotRange : RouletteSlotBase
    {
        [SerializeField] private bool matchLess;
        public override bool CheckMatch(RouletteNumber rouletteNum, ref int bet)
        {
            return !(matchLess ^ rouletteNum.Number < 19);
        }
    }
}
