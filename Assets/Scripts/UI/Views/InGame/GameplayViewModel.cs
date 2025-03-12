using Cysharp.Threading.Tasks;
using TSoft.InGame;
using TSoft.InGame.Player;

namespace TSoft.UI.Views.InGame
{
    public class GameplayViewModel : ViewModel
    {
        private GameplayView View => view as GameplayView;
        private GameplayModel Model => model as GameplayModel;

        private PlayerController player;

        private void Start()
        {
            player = Model.Player;
            
            View.DiscardButton.onClick.AddListener(OnDiscardCard);
            View.UseButton.onClick.AddListener(() => OnUseCard().Forget());
            
            player.Gameplay.GetAttrVar(GameplayAttr.Heart).OnValueChanged += OnPlayerHeartChanged;
            player.Gameplay.GetAttrVar(GameplayAttr.Energy).OnValueChanged += OnPlayerEnergyChanged;
            
            //TODO 이벤트 버스로 변경
            player.onDeckChanged += OnDeckChanged;
            player.onGameReady += UpdateCardOnGameReady;
        }
        
        private void OnPlayerHeartChanged(float oldVal, float newVal)
        {
            var maxCount = player.Gameplay.GetAttr(GameplayAttr.MaxHeart);
            View.SetHeartText(newVal, maxCount);
        }
        
        private void OnPlayerEnergyChanged(float oldVal, float newVal)
        {
            var maxCount = player.Gameplay.GetAttr(GameplayAttr.MaxEnergy);
            View.SetEnergyText(newVal, maxCount);
        }
        
        private void OnDiscardCard()
        {
            if(!player.TryDiscardSelectedCard())
                return;
            
            player.DrawCards();
        }
        
        private async UniTaskVoid OnUseCard()
        {
            var result = await player.TryUseCardsOnHand();
            if (!result)
                return;
            
            player.DrawCards();
        }
        
        private void OnDeckChanged(int cardNum, int maxCardNum)
        {
            View.SetDeckText(cardNum, maxCardNum);
        }
        
        private void UpdateCardOnGameReady()
        {
            var maxEnergyCount = player.Gameplay.GetAttr(GameplayAttr.MaxEnergy);
            var energyCount = player.Gameplay.GetAttr(GameplayAttr.Energy);
            var maxHeartCount = player.Gameplay.GetAttr(GameplayAttr.MaxHeart);
            var heartCount = player.Gameplay.GetAttr(GameplayAttr.Heart);
            
            View.SetEnergyText(energyCount, maxEnergyCount);
            View.SetHeartText(heartCount, maxHeartCount);
            
            player.DrawCards();
        }
    }
}
