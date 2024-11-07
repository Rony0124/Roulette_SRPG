namespace TSoft.InGame.Roulette
{
    public interface IRouletteSlotHandler
    {
        public bool CheckMatch(RouletteNumber rouletteNum, ref int bet);
    }
}
