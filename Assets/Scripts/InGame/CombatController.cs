using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TSoft.InGame
{
    public class CombatController : MonoBehaviour
    {
        //life cycle 동기화 flag
        private GameState currentGameState;
        private StageState currentStageState;
        //field
        private FieldController currentField;
        //round
        private int round;
        
        public GameState CurrentGameState => currentGameState;
        public StageState CurrentStageState => currentStageState;
        public FieldController CurrentField => currentField;
        
        public InGameDirector Director { get; set; }
        
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

            currentStageState = newVal;
        }

        #region Stage
        
        private async UniTask OnPrePlay()
        {
            currentField = GameContext.Instance.StageRegistry.SpawnNextStage(transform);
            
            await UniTask.WaitForSeconds(1);
        }
        
        #endregion
        
        #region

        private async UniTask OnGameReady()
        {
            round++;
            
            var mIndex = currentField.CurrentSlotIndex;
            Director.CurrentMonsters = currentField.Slots[mIndex].monsters;
            
            Debug.Log("current round" + round);
            
            await UniTask.WaitForSeconds(1);
        }
        
        #endregion


    }
}
