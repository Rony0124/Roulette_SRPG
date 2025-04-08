using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TSoft.Managers;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TSoft.InGame
{
   public class InGameDirector : DirectorBase
    {
        [Header("Combat")] 
        [SerializeField] private CombatController combat;
        public CombatController Combat => combat;
        
        [Header("Feedbacks")] 
        public UnityEvent introFeedback;
        public UnityEvent prePlayFeedback;
        public UnityEvent postPlaySuccessFeedback;
        public UnityEvent postPlayFailFeedback;
        public UnityEvent outroFeedback;

        //life cycle
        public ObservableVar<StageState> CurrentStageState;

        protected override void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue)
        {
            CurrentStageState = new ObservableVar<StageState>();
            CurrentStageState.OnValueChanged +=  OnStageStateChanged;
            
            combat.Director = this;
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

        public void SetStageState(int stageStage)
        {
            CurrentStageState.Value = (StageState)stageStage;
        }
        
        private async UniTaskVoid IntroAsync()
        {
            //TODO 타임라인을 깔수도 있다.
            await UniTask.Delay(100);
            
            CurrentStageState.Value = StageState.PrePlaying;
        }

        private async UniTaskVoid PrePlayAsync()
        {
            await Combat.GameStart();
            
            await UniTask.Delay(1000);
            
            CurrentStageState.Value = StageState.Playing;
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
            Combat.StartTurn().Forget();
        }

        private void OnPostPlayingSuccess()
        {
            postPlaySuccessFeedback?.Invoke();
        }
        
        private void OnPostPlayingFail()
        {
            postPlayFailFeedback?.Invoke();
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
