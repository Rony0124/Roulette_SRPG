using HF.Utils;

namespace HF.GamePlay
{
    public sealed class GameLogic
    {
        private Game gameData;
        
        private ResolveQueue resolveQueue;
        private bool isAi;
        
        public GameLogic(bool isAi)
        {
            resolveQueue = new ResolveQueue(null, isAi);
            this.isAi = isAi;
        }
        
        public void SetData(Game game)
        {
            gameData = game;
            resolveQueue.SetData(game);
        }
        
        public void ClearResolve()
        {
            resolveQueue.Clear();
        }
        
        public bool IsResolving()
        {
            return resolveQueue.IsResolving();
        }
        
        public Game GetGameData()
        {
            return gameData;
        }
    }
}
