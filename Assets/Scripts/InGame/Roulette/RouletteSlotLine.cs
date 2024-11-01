namespace TSoft.InGame.Roulette
{
    public class RouletteSlotLine : RouletteSlotBase
    {
        private int matchNumber;
        
        public override bool CheckMatch(RouletteNumber rouletteNum, ref RouletteController.Bet bet)
        {
            var rulNum = rouletteNum.Number;
            if (rulNum % 3 != matchNumber) 
                return false;
            
            bet.Wager += Wager * 3;
            return true;

        }
    }
}
