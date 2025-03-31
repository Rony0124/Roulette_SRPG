using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HF.AI;
using HF.GamePlay;
using InGame;
using TSoft;
using TSoft.InGame;
using TSoft.Managers;
using TSoft.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using PlayerSettings = HF.GamePlay.PlayerSettings;

namespace HF.InGame
{
    public class InGameDirector : DirectorBase
    {
        [Header("Combat")] 
        [SerializeField] private CombatController combat;
        public CombatController Combat => combat;

        private Game gameData; 
        private GameLogic gameplay;
        //test
        public static PlayerSettings ai_settings = PlayerSettings.DefaultAI;
        private List<AIPlayer> aiList = new ();                //List of all AI players
        public Queue<AIAction> queuedAction = new();

        [Header("Feedbacks")] 
        public UnityEvent introFeedback;
        public UnityEvent prePlayFeedback;
        public UnityEvent postPlaySuccessFeedback;
        public UnityEvent postPlayFailFeedback;
        public UnityEvent outroFeedback;

        //life cycle
        private ObservableVar<StageState> currentStageState;

        public StageState CurrentStageState => currentStageState.Value;

        protected override void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue)
        {
            combat.Director = this;
            
            currentStageState = new ObservableVar<StageState>();
            currentStageState.OnValueChanged +=  OnStageStateChanged;

            //TODO test
            gameData = new Game("userTest", 2);
            gameplay = new GameLogic(gameData);
            
            currentStageState.Value = StageState.Intro;
        }

        private void OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            if (oldVal == newVal)
                return;

            Debug.Log($"Current Stage State [{newVal}]");

            switch (newVal)
            {
                case StageState.Intro:
                    OnIntro();
                    break;
                case StageState.PrePlaying:
                    OnPrePlay();
                    break;
                case StageState.Playing:
                    OnPlay();
                    break;
                case StageState.PostPlayingSuccess:
                    OnPostPlayingSuccess();
                    break;
                case StageState.PostPlayingFailed:
                    OnPostPlayingFail();
                    break;
                case StageState.Outro:
                    OnOutro();
                    break;
                case StageState.Exit:
                    OnExit();
                    break;
            }
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

        public void SetStageState(int stageStage)
        {
            currentStageState.Value = (StageState)stageStage;
        }
        
        private async UniTaskVoid IntroAsync()
        {
            //TODO 타임라인을 깔수도 있다. 현재는 몬스터의 인트로 피드백이 끝날때까지 기다리는 방식
            await UniTask.WaitUntil(() => combat.CurrentMonster != null);
            await combat.CurrentMonster.OnIntro();
            
            currentStageState.Value = StageState.PrePlaying;
        }

        private async UniTaskVoid PrePlayAsync()
        {
            if (gameData.state == GameState.GameEnded)
                return;
            
            GameStart();
            
            //TODO 몬스터, 플레이어 카드 및 아이템 소환
            var player = combat.Player;
            await player.OnPrePlay();
            
            currentStageState.Value = StageState.Playing;
        }

        private void GameStart()
        {
            Debug.Log("Game start");
            //TODO test 현재 offline으로 연결되었다는 가정하에 플레이
            SetPlayerSettingsAI(0, ai_settings);
            
            //Setup AI
            foreach (Player player in gameData.players)
            {
                if (player.is_ai)
                {
                    AIPlayer ai_gameplay = AIPlayer.Create(AIType.MiniMax, gameplay, player.player_id);
                    aiList.Add(ai_gameplay);
                    
                    if(ai_gameplay != null)
                        Debug.Log("AI Created");
                }
            }

            //Start Game
            gameplay.StartGame();
        }
        
        public void EndTurn(int playerId)
        {
            Player player = gameData.GetPlayer(playerId);
            if (player != null && gameData.IsPlayerTurn(player))
            {
                Debug.Log("move to next step");
                gameplay.NextStep();
            }
        }

        public void GameOver(bool isSuccess)
        {
            if (isSuccess)
            {
                SetStageState((int)StageState.PostPlayingSuccess);
            }else
            {
                SetStageState((int)StageState.PostPlayingFailed);
            }
        }

        public void GameFinishSuccess()
        {
            PopupContainer.Instance.ClosePopupUI();
            //TODO 보상 시스템 제대로 만들자
            GameSave.Instance.AddGold((int)GameContext.Instance.currentBounty);
            
            SceneManager.LoadScene(Define.Lobby);
        }

        public void GameFinishFail()
        {
            PopupContainer.Instance.ClosePopupUI();

            GameSave.Instance.ClearSaveFile();
            
            SceneManager.LoadScene(Define.Lobby);
        }
        
        private void SetPlayerSettingsAI(int player_id, PlayerSettings psettings)
        {
            if (gameData.state != GameState.Connecting)
                return; //Cant send setting if game already started

            Player player = gameData.GetOpponentPlayer(player_id);
            if (player is { ready: false })
            {
                player.is_ai = true;
                player.ready = true;

                //SetPlayerDeck(player.player_id, player.username, psettings.deck);
                //RefreshAll();
            }
            
            Debug.Log("Set AI Opponent");
        }

        #region Stage

        //입장 인트로
        private void OnIntro()
        {
            introFeedback?.Invoke();
            IntroAsync().Forget();
        }

        private void OnPrePlay()
        {
            prePlayFeedback?.Invoke();
            PrePlayAsync().Forget();
        }

        private void OnPlay()
        {
            
        }

        private void OnPostPlayingSuccess()
        {
            postPlaySuccessFeedback?.Invoke();
        }
        
        private void OnPostPlayingFail()
        {
            postPlaySuccessFeedback?.Invoke();
        }

        private void OnOutro()
        {
            outroFeedback?.Invoke();
        }

        private void OnExit()
        {
            Debug.Log("Game is Over. Exit the game");
        }

        #endregion
    }
}
