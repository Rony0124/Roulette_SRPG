using System.Collections.Generic;
using HF.AI;
using HF.GamePlay;
using HF.Utils;
using InGame;
using UnityEngine;

namespace HF.InGame
{
    public class CombatController : ControllerBase
    {
        //GamePlay
        private Game gameData;
        private GameLogic gameplay;
        
        private List<AIPlayer> aiList = new ();
        
        public static int OwnerPlayerId = 0;
        public static PlayerSettings AISettings = PlayerSettings.DefaultAI;

        protected override void OnDirectorChanged()
        {
            gameData = new Game("userTest", 2);
            gameplay = new GameLogic(gameData);
         
#if UNITY_EDITOR
            if (HfDevPreferences.Ai)
            {
                var oPlayer = gameData.GetOpponentPlayer(OwnerPlayerId);
            }
            
            SetPlayerSettingsAI(OwnerPlayerId, AISettings);
#endif
        }
        
        //TODO 턴변경 시스템 추가
        private void Update()
        {
            //Update game logic
            gameplay.Update(Time.deltaTime);
            
            //Update AI
            foreach (AIPlayer ai in aiList)
            {
                ai.Update();
            }
        }
        
        public void GameStart()
        {
            if(gameData.state == GameState.GameEnded)
                return;
            
            //Setup AI
            foreach (var player in gameData.players)
            {
                if (player.is_ai)
                {
                    AIPlayer ai_gameplay = AIPlayer.Create(AIType.MiniMax, gameplay, player.player_id);
                    aiList.Add(ai_gameplay);
                }
            }

            //Start Game
            gameplay.StartGame();
        }
        
        public void EndTurn(int playerId)
        {
            GamePlay.Player player = gameData.GetPlayer(playerId);
            if (player != null && gameData.IsPlayerTurn(player))
            {
                gameplay.NextStep();
            }
        }
        
        private void SetPlayerSettingsAI(int player_id, PlayerSettings psettings)
        {
            if (gameData.state != GameState.Connecting)
                return;

            GamePlay.Player player = gameData.GetOpponentPlayer(player_id);
            if (player is { ready: false })
            {
                player.is_ai = true;
                player.ready = true;

                //SetPlayerDeck(player.player_id, player.username, psettings.deck);
                //RefreshAll();
            }
            
            Debug.Log("Set AI Opponent");
        }

    }
}
