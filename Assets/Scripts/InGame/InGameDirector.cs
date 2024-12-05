using System;
using TSoft.Utils;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.InGame
{
    public class InGameDirector : DirectorBase
    {
        //game play
        private MonsterController currentMonster;
        private PlayerController currentPlayer;
        
        //game life cycle
        private ObservableVar<GameState> currentGameState;

        public MonsterController CurrentMonster => currentMonster;
        public PlayerController CurrentPlayer => currentPlayer;
        public GameState CurrentGameState => currentGameState.Value;

        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            
            currentPlayer = FindObjectOfType<PlayerController>();
            currentGameState.OnValueChanged += OnGameStateChanged;
        }
        
        private void OnGameStateChanged(GameState oldVal, GameState newVal)
        {
            if (oldVal == newVal)
                return;

            switch (newVal)
            {
                case GameState.PrePlaying:
                    break;
                case GameState.Playing:
                    break;
                case GameState.PostPlaying:
                    break;
                case GameState.OutroSuccess:
                    break;
                case GameState.OutroFailed:
                    break;
                case GameState.OutroReset:
                    break;
                case GameState.Exit:
                    break;
            }
        }
    }
}
