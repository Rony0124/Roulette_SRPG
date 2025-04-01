using Cysharp.Threading.Tasks;
using InGame;
using TSoft;
using TSoft.InGame;
using TSoft.Managers;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HF.InGame
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
        private ObservableVar<StageState> currentStageState;

        public StageState CurrentStageState => currentStageState.Value;

        protected override void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue)
        {
            combat.Director = this;
            
            currentStageState = new ObservableVar<StageState>();
            currentStageState.OnValueChanged +=  OnStageStateChanged;
            
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

        public void SetStageState(int stageStage)
        {
            currentStageState.Value = (StageState)stageStage;
        }
        
        private async UniTaskVoid IntroAsync()
        {
            //TODO 타임라인을 깔수도 있다. 현재는 몬스터의 인트로 피드백이 끝날때까지 기다리는 방식
            await UniTask.Delay(100);
            
            currentStageState.Value = StageState.PrePlaying;
        }

        private async UniTaskVoid PrePlayAsync()
        {
            Combat.GameStart();
            
            //TODO 몬스터, 플레이어 카드 및 아이템 소환
            
            currentStageState.Value = StageState.Playing;
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
