using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TSoft.InGame
{
    public abstract class ControllerBase : MonoBehaviour
    {
        //life cycle 동기화 flag
        protected GameState currentGameState;
        protected StageState currentStageState;
        //director
        protected InGameDirector director;
        
        public GameState CurrentGameState => currentGameState;
        public StageState CurrentStageState => currentStageState;
        
        public InGameDirector Director
        {
            get => director;
            set
            {
                InitOnDirectorChanged();
                director = value;
            }
        }

        protected abstract void InitOnDirectorChanged();
        
        public async UniTaskVoid OnGameStateChanged(GameState oldVal, GameState newVal)
        {
            switch (newVal)
            {
                case GameState.Ready:
                    await OnGameReady();
                    break;
                case GameState.Play:
                    await OnGamePlay();
                    break;
                case GameState.FinishSuccess:
                    await OnGameFinishSuccess();
                    break;
                case GameState.FinishFailed:
                    await OnGameFinishFailed();
                    break;
            }

            currentGameState = newVal;
        }
        
        public async UniTaskVoid OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            switch (newVal)
            {
                case StageState.Intro:
                    await OnIntro();
                    break;
                case StageState.PrePlaying:
                    await OnPrePlay();
                    break;
                case StageState.Playing:
                    await OnPlay();
                    break;
                case StageState.PostPlayingSuccess:
                    await OnPostPlaySuccess();
                    break;
                case StageState.PostPlayingFailed:
                    await OnPostPlayFailed();
                    break;
                case StageState.Outro:
                    await OnOutro();
                    break;
                case StageState.Exit:
                    break;
            }

            currentStageState = newVal;
        }

        #region Game

        protected virtual async UniTask OnGameReady()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnGamePlay()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnGameFinishSuccess()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnGameFinishFailed()
        {
            await UniTask.CompletedTask;
        }

        #endregion
        
        
        #region Stage

        protected virtual async UniTask OnIntro()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnPrePlay()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnPlay()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnPostPlaySuccess()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnPostPlayFailed()
        {
            await UniTask.CompletedTask;
        }
        
        protected virtual async UniTask OnOutro()
        {
            await UniTask.CompletedTask;
        }

        #endregion
    
    }
}
