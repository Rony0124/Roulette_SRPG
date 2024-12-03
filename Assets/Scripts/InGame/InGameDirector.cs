using TSoft.InGame.CardSystem;

namespace TSoft.InGame
{
    public class InGameDirector : DirectorBase
    {
        private MonsterController currentMonster;
        private PlayerController currentPlayer;
        
        public MonsterController CurrentMonster => currentMonster;
        public PlayerController CurrentPlayer => currentPlayer;

        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            
            currentPlayer = FindObjectOfType<PlayerController>();
        }
    }
}
