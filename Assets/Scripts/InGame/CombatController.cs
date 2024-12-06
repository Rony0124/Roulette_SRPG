using UnityEngine;

namespace TSoft.InGame
{
    public class CombatController : MonoBehaviour
    {
        [SerializeField] private GameObject fieldPrefab;
        
        //life cycle 동기화 flag
        private GameState currentGameState;
        private StageState currentStageState;
        
        public void OnGameStateChanged(GameState oldVal, GameState newVal)
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
        }
        
        private void OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            switch (newVal)
            {
                case StageState.Intro:
                    break;
                case StageState.PrePlaying:
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

    }
}
