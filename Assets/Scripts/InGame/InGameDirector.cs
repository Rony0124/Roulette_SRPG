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
        
        //game play
        private MonsterController currentMonster;
        private PlayerController currentPlayer;
        private CombatController combatController;
        
        //life cycle
        private ObservableVar<GameState> currentGameState;
        private ObservableVar<StageState> currentStageState;

        public MonsterController CurrentMonster => currentMonster;
        public PlayerController CurrentPlayer => currentPlayer;

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
        }
        
        private void OnGameStateChanged(GameState oldVal, GameState newVal)
        {
            if (oldVal == newVal)
                return;

            Debug.Log($"Current Game State [{newVal}]");
            
            switch (newVal)
            {
                case GameState.Ready:
                    break;
                case GameState.Play:
                    break;
                case GameState.FinishSuccess:
                    break;
                case GameState.FinishFailed:
                    break;
            }

            combatController.OnGameStateChanged(oldVal, newVal);
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
                case StageState.PostPlaying:
                    break;
                case StageState.OutroSuccess:
                    break;
                case StageState.OutroFailed:
                    break;
                case StageState.OutroReset:
                    break;
                case StageState.Exit:
                    break;
            }
        }
        
        private async UniTaskVoid StartIntro()
        {
            await UniTask.WaitForSeconds(3);
            
            currentStageState.Value = StageState.PrePlaying;
        }

        private async UniTaskVoid StartPrePlaying()
        {
            OnPrePlay?.Invoke();
            
            await UniTask.WaitForSeconds(1);
            
            currentStageState.Value = StageState.Playing;
        }
    }
}
