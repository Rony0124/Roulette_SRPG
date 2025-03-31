using TSoft.GameLogic;
using UnityEngine;

namespace HF.AI
{
    public class AILogic
    {
        public int aiPlayerId;
        
        private GameLogic gameLogic; 
        private AIHeuristic heuristic;
        
        public static AILogic Create(int player_id)
        {
            var job = new AILogic
            {
                aiPlayerId = player_id
            };

            job.heuristic = new AIHeuristic(player_id);
            job.gameLogic = new GameLogic(true); //Skip all delays for the AI calculations

            return job;
        }
    }
}
