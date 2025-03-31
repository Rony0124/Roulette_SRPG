using HF.GamePlay;

namespace HF.AI
{
    public abstract class AIPlayer
    {
        public int playerId;
        
        protected GameLogic gameplay;

        public abstract void Update();
        
        public bool CanPlay()
        {
            var gameData = gameplay.GetGameData();
            var player = gameData.GetPlayer(playerId);
            var canPlay = gameData.IsPlayerTurn(player);
            
            return canPlay && !gameplay.IsResolving();
        }
        
        public static AIPlayer Create(AIType type, GameLogic gameplay, int id)
        {
            if (type == AIType.Random)
                return new AIPlayerRandom(gameplay, id);
            if (type == AIType.MiniMax)
                return new AIPlayerMM(gameplay, id);
            return null;
        }
    }
    
    public enum AIType
    {
        Random = 0,      //Dumb AI that just do random moves, useful for testing cards without getting destroyed
        MiniMax = 10,    //Stronger AI using Minimax algo with alpha-beta pruning
    }
}
