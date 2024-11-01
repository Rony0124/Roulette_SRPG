namespace TSoft.InGame.Roulette
{
    public interface IRouletteSlotHandler
    {
        public bool IsBet();
        public bool CheckMatch(RouletteNumber rouletteNum, ref RouletteController.Bet bet);
    }
}
