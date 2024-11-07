using UnityEngine;

namespace TSoft.InGame.Roulette
{
    public class RouletteSlotSection : RouletteSlotBase
    {
        [SerializeField] private SectionType matchType;
        public override bool CheckMatch(RouletteNumber rouletteNum, ref int bet)
        {
            return matchType == rouletteNum.SectionType;
        }
    }
}
