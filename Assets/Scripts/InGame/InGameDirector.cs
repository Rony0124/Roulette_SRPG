using TSoft;
using TSoft.InGame;
using TSoft.Managers;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace InGame
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
        }

        private void OnPrePlay()
        {
            prePlayFeedback?.Invoke();
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
