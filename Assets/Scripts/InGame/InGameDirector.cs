using System;
using Cysharp.Threading.Tasks;
using TSoft.Utils;
using UnityEngine;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.InGame
{
    public class InGameDirector : DirectorBase
    {
        public Action OnPrePlay;
        public Action OnGameReady;
        
        //game play
        private PlayerController player;
        private CombatController combat;
        
        //life cycle
        private ObservableVar<GameState> currentGameState;
        private ObservableVar<StageState> currentStageState;
        
        public PlayerController Player => player;
        public FieldController CurrentField => combat.CurrentField;

        public GameState CurrentGameState => currentGameState.Value;
        public StageState CurrentStageState => currentStageState.Value;

        protected override void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue)
        {
            //TODO 로드 타이밍 수정!!
            //DataRegistry.instance.Load().Forget();
            
            player = FindObjectOfType<PlayerController>();
            combat = FindObjectOfType<CombatController>();

            currentGameState = new ObservableVar<GameState>();
            currentStageState = new ObservableVar<StageState>();
            
            currentGameState.OnValueChanged += (o, n) => OnGameStateChanged(o, n).Forget();
            currentStageState.OnValueChanged += (o, n) => OnStageStateChanged(o, n).Forget();;
            
            currentStageState.Value = StageState.Intro;

            combat.Director = this;
            player.Director = this;
        }
        
        private async UniTaskVoid OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            if (oldVal == newVal)
                return;

            Debug.Log($"Current Stage State [{newVal}]");

            if (oldVal != StageState.None)
            {
                //controller cycle 동기화
                await UniTask.WaitUntil(() => combat.CurrentStageState == oldVal);
                await UniTask.WaitUntil(() => player.CurrentStageState == oldVal);    
            }
            
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
            
            player.OnStageStateChanged(oldVal, newVal).Forget();
            combat.OnStageStateChanged(oldVal, newVal).Forget();
        }
        
        private async UniTaskVoid OnGameStateChanged(GameState oldVal, GameState newVal)
        {
            if (oldVal == newVal)
                return;

            if (oldVal != GameState.None)
            {
                await UniTask.WaitUntil(() => combat.CurrentGameState == oldVal);
                await UniTask.WaitUntil(() => player.CurrentGameState == oldVal);
            }
            
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
            
            player.OnGameStateChanged(oldVal, newVal).Forget();
            combat.OnGameStateChanged(oldVal, newVal).Forget();
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
            
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.Playing);
            ChangeGameState(GameState.Ready);
        }
        
        //스테이지 마무리 준비 (성공)
        private async UniTaskVoid StartPostPlayingSuccess()
        {
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.PrePlaying);
        }
        
        //스테이지 마무리 준비 (실패)
        private async UniTaskVoid StartPostPlayingFailed()
        {
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.Outro);
        }

        #endregion

        #region Game

        private async UniTaskVoid StartGameReady()
        {
            OnGameReady?.Invoke();
            
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
            await UniTask.WaitForSeconds(1);
            
            if (combat.CurrentCycleInfo.IsRoundMax)
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
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.PostPlayingFailed);
        }

        #endregion
    }
}
