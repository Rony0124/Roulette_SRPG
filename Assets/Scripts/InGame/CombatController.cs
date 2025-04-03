using System.Collections.Generic;
using HF.AI;
using HF.Data;
using HF.Data.Card;
using HF.GamePlay;
using HF.Utils;
using InGame;
using UnityEngine;

namespace HF.InGame
{
    public class CombatController : ControllerBase
    {
        [Header("test")] 
        public GameplayData gpData;
        
        //GamePlay
        private Game gameData;
        private GameLogic gameplay;
        
        private List<AIPlayer> aiList = new ();

        public Game GameData => gameData;
        //temp
        public static int OwnerPlayerId = 0;
        public static int MaxCardCapacity = 9;
        public static PlayerSettings AISettings = PlayerSettings.DefaultAI;

        protected override void OnDirectorChanged()
        {
            gameData = new Game("userTest", 2);
            gameplay = new GameLogic(gameData);
         
#if UNITY_EDITOR
            SetPlayerSettings(OwnerPlayerId, gpData.test_deck);
            
            if (HfDevPreferences.Ai)
            {
                SetPlayerSettingsAI(OwnerPlayerId, gpData.test_deck_ai);
            }
#endif
        }
        
        //TODO 턴변경 시스템 추가하기
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
            var player = gameData.GetPlayer(playerId);
            if (player != null && gameData.IsPlayerTurn(player))
            {
                gameplay.NextStep();
            }
        }
        
        private void SetPlayerSettings(int playerID, DeckData deck)
        {
            if (gameData.state != GameState.Connecting)
                return;

            var player = gameData.GetPlayer(playerID);
            if (player is not { ready: false }) 
                return;
            
            player.is_ai = false;
            player.ready = true;
                
            SetPlayerDeck(player, deck);
        }
        
        private void SetPlayerSettingsAI(int playerID, DeckData deck)
        {
            if (gameData.state != GameState.Connecting)
                return;

            var player = gameData.GetOpponentPlayer(playerID);
            if (player is not { ready: false })
                return;
            
            player.is_ai = true;
            player.ready = true;

            SetPlayerDeck(player, deck);
        }
        
        private void SetPlayerDeck(GamePlay.Player player, DeckData deck)
        {
            if (player == null)
                return;
            
            gameplay.SetPlayerDeck(player, deck);
        }

    }
}
