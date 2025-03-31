using Cysharp.Threading.Tasks;
using HF.GamePlay;
using UnityEngine;

namespace HF.AI
{
    public class AIPlayerMM : AIPlayer
    {
        private AILogic aiLogic;
        
        private bool isPlaying = false;
        
        public AIPlayerMM(GameLogic gameplay, int id)
        {
            this.gameplay = gameplay;
            playerId = id;
            aiLogic = AILogic.Create(id);
        }
        
        public override void Update()
        {
            Game game_data = gameplay.GetGameData();
            Player player = game_data.GetPlayer(playerId);

            if (!isPlaying && game_data.IsPlayerTurn(player))
            {
                isPlaying = true;
                AiTurn().Forget();
            }

            /*if (!isPlaying && game_data.IsPlayerMulliganTurn(player))
            {
                SkipMulligan();
            }*/

            if (!game_data.IsPlayerTurn(player) && aiLogic.IsRunning())
                Stop();
        }
        
        private async UniTaskVoid AiTurn()
        {
            await UniTask.WaitForSeconds(1);

            var gameData = gameplay.GetGameData();
            aiLogic.RunAI(gameData);

            while (aiLogic.IsRunning())
            {
                await UniTask.WaitForSeconds(0.1f);
            }

            await UniTask.WaitWhile(() => aiLogic.IsRunning());

            var best = aiLogic.GetBestAction();

            if (best != null)
            {
                Debug.Log("Execute AI Action: " + best.GetText(gameData) + "\n" + aiLogic.GetNodePath());
                //foreach (NodeState node in ai_logic.GetFirst().childs)
                //   Debug.Log(ai_logic.GetNodePath(node));

                ExecuteAction(best);
            }

            aiLogic.ClearMemory();

            await UniTask.WaitForSeconds(0.5f);
            isPlaying = false;
        }
        
        private void Stop()
        {
            aiLogic.Stop();
            isPlaying = false;
        }
        
        private void ExecuteAction(AIAction action)
        {
            if (!CanPlay())
                return;
            
            //TODO action 정의해주기
            if (action.type == GameAction.EndTurn)
            {
                EndTurn();
            }
        }
        
        private void EndTurn()
        {
            if (CanPlay())
            {
                gameplay.EndTurn();
            }
        }
    }
}
