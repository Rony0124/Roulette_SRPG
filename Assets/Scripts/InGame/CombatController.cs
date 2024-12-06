using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TSoft.InGame
{
    public class CombatController : MonoBehaviour
    {
        [SerializeField] private GameObject fieldPrefab;
        
        //life cycle 동기화 flag
        private GameState currentGameState;
        private StageState currentStageState;
        
        public GameState CurrentGameState => currentGameState;
        public StageState CurrentStageState => currentStageState;
        
        public async UniTaskVoid OnGameStateChanged(GameState oldVal, GameState newVal)
        {
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

        private async UniTask OnPrePlay()
        {
            Debug.Log("prepping play from combat");
            var field = Instantiate(fieldPrefab, transform).GetComponent<StageController>();
            field.SpawnField();
            
            await UniTask.WaitForSeconds(1);
        }
    }
}
