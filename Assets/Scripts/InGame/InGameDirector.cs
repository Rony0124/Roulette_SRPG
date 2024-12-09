using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSoft.Data.Registry;
using TSoft.Utils;
using UnityEngine;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.InGame
{
    public class InGameDirector : DirectorBase
    {
        public Action OnPrePlay;
        
        //game play
        private PlayerController currentPlayer;
        private CombatController combatController;
        
        //life cycle
        private ObservableVar<GameState> currentGameState;
        private ObservableVar<StageState> currentStageState;
        
        public PlayerController CurrentPlayer => currentPlayer;
        public FieldController CurrentField => combatController.CurrentField;

        public GameState CurrentGameState => currentGameState.Value;
        public StageState CurrentStageState => currentStageState.Value;

        protected override void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue)
        {
            currentPlayer = FindObjectOfType<PlayerController>();
            combatController = FindObjectOfType<CombatController>();

            currentGameState = new ObservableVar<GameState>();
            currentStageState = new ObservableVar<StageState>();
            
            currentGameState.OnValueChanged += OnGameStateChanged;
            currentStageState.OnValueChanged += OnStageStateChanged;
            
            currentStageState.Value = StageState.Intro;

            combatController.Director = this;
        }
        
        private void OnGameStateChanged(GameState oldVal, GameState newVal)
        {
            if (oldVal == newVal)
                return;

            Debug.Log($"Current Game State [{newVal}]");
            
            switch (newVal)
            {
                case GameState.Ready:
                    StartGameReady().Forget();
                    break;
                case GameState.Play:
                    break;
                case GameState.FinishSuccess:
                    StartGameFinishSuccess().Forget();
                    break;
                case GameState.FinishFailed:
                    StartGameFinishFailure().Forget();
                    break;
            }

            combatController.OnGameStateChanged(oldVal, newVal).Forget();
        }

        private void OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            if (oldVal == newVal)
                return;

            Debug.Log($"Current Stage State [{newVal}]");
            
            switch (newVal)
            {
                case StageState.Intro:
                    StartIntro().Forget();
                    break;
                case StageState.PrePlaying:
                    StartPrePlaying().Forget();
                    break;
                case StageState.Playing:
                    break;
                case StageState.PostPlayingSuccess:
                    StartPostPlayingSuccess().Forget();
                    break;
                case StageState.PostPlayingFailed:
                    StartPostPlayingFailed().Forget();
                    break;
                case StageState.Outro:
                    break;
                case StageState.Exit:
                    break;
            }
            
            combatController.OnStageStateChanged(oldVal, newVal).Forget();
        }

        public void ChangeStageState(StageState stageState)
        {
            currentStageState.Value = stageState;
        }
        
        public void ChangeGameState(GameState gameState)
        {
            currentGameState.Value = gameState;
        }

        public void GameOver(bool isSuccess)
        {
            Debug.Log("GameOver");

            if (isSuccess)
            {
                ChangeGameState(GameState.FinishSuccess);
            }else
            {
                ChangeGameState(GameState.FinishFailed);
            }
        }

        #region Stage

        //입장 인트로
        private async UniTaskVoid StartIntro()
        {
            await UniTask.WaitForSeconds(3);
            
            ChangeStageState(StageState.PrePlaying);
        }

        //스테이지 준비
        private async UniTaskVoid StartPrePlaying()
        {
            OnPrePlay?.Invoke();

            await UniTask.WaitUntil(() => combatController.CurrentStageState == currentStageState.Value);
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.Playing);
            ChangeGameState(GameState.Ready);
        }
        
        //스테이지 마무리 준비 (성공)
        private async UniTaskVoid StartPostPlayingSuccess()
        {
            await UniTask.WaitUntil(() => combatController.CurrentStageState == currentStageState.Value);
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.PrePlaying);
        }
        
        //스테이지 마무리 준비 (실패)
        private async UniTaskVoid StartPostPlayingFailed()
        {
            await UniTask.WaitUntil(() => combatController.CurrentStageState == currentStageState.Value);
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.Outro);
        }

        #endregion

        #region Game

        private async UniTaskVoid StartGameReady()
        {
            await UniTask.WaitUntil(() => combatController.CurrentGameState == currentGameState.Value);
            await UniTask.WaitForSeconds(1);
            
            ChangeGameState(GameState.Play);
        }

        //스테이지 준비
        private async UniTaskVoid StartGamePlay()
        {
            await UniTask.WaitForSeconds(1);
        }
        
        private async UniTaskVoid StartGameFinishSuccess()
        {
            await UniTask.WaitUntil(() => combatController.CurrentGameState == currentGameState.Value);
            await UniTask.WaitForSeconds(1);

            if (combatController.CurrentCycleInfo.IsRoundMax)
            {
                ChangeStageState(StageState.PostPlayingSuccess);
            }
            else
            {
                ChangeGameState(GameState.Ready);
            }
                
        }
        
        private async UniTaskVoid StartGameFinishFailure()
        {
            await UniTask.WaitUntil(() => combatController.CurrentGameState == currentGameState.Value);
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.PostPlayingFailed);
        }

        #endregion
    }
}
