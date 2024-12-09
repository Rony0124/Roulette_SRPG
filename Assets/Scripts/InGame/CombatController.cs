using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TSoft.InGame
{
    public class CombatController : MonoBehaviour
    {
        public struct CycleInfo
        {
            public int Round;
            public int Stage;
            public bool IsRoundMax => Round >= 5;
            
            public void Reset()
            {
                Round = 0;
                Stage = 0;
            }
        }
        
        //life cycle 동기화 flag
        private GameState currentGameState;
        private StageState currentStageState;
        //field
        private FieldController currentField;
        //cycle
        private CycleInfo currentCycleInfo;

        private InGameDirector director; 
        
        public GameState CurrentGameState => currentGameState;
        public StageState CurrentStageState => currentStageState;
        public FieldController CurrentField => currentField;
        public CycleInfo CurrentCycleInfo => currentCycleInfo;
        
        public InGameDirector Director
        {
            get => director;
            set
            {
                ResetOnDirectorChanged();
                director = value;
            }
        }

        private void ResetOnDirectorChanged()
        {
            currentCycleInfo.Reset();
        }

        public async UniTaskVoid OnGameStateChanged(GameState oldVal, GameState newVal)
        {
            switch (newVal)
            {
                case GameState.Ready:
                    await OnGameReady();
                   
                    break;
                case GameState.Play:
                    break;
                case GameState.FinishSuccess:
                    break;
                case GameState.FinishFailed:
                    break;
            }

            currentGameState = newVal;
        }
        
        public async UniTaskVoid OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            switch (newVal)
            {
                case StageState.Intro:
                    break;
                case StageState.PrePlaying:
                    await OnPrePlay();
                    break;
                case StageState.Playing:
                    break;
                case StageState.PostPlayingSuccess:
                    break;
                case StageState.PostPlayingFailed:
                    break;
                case StageState.Outro:
                    break;
                case StageState.Exit:
                    break;
            }

            currentStageState = newVal;
        }

        #region Stage
        
        private async UniTask OnPrePlay()
        {
            currentCycleInfo.Round = 0;
            currentCycleInfo.Stage++;
            
            currentField = GameContext.Instance.StageRegistry.SpawnNextStage(transform, currentCycleInfo.Stage);
            currentField.SetFieldData(currentCycleInfo);
            
            await UniTask.WaitForSeconds(1);
        }
        
        #endregion
        
        #region

        private async UniTask OnGameReady()
        {
            currentCycleInfo.Round++;
            
            var mIndex = currentCycleInfo.Round;
            currentField.CurrentSlotIndex = mIndex;
            
            Debug.Log("current round" + currentCycleInfo.Round);
            
            await UniTask.WaitForSeconds(1);
        }
        
        #endregion


    }
}
