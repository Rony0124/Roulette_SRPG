using TSoft.InGame.Player;
using TSoft.UI.Views;
using TSoft.UI.Views.InGame;

namespace UI.Views.InGame
{
    public class AttackInfoViewModelBase : ViewModelBase
    {
        private AttackInfoView View => view as AttackInfoView;
        private AttackInfoModel Model => model as AttackInfoModel;
        
        void Start()
        {
            Model.Player.onPatternSelected += OnCombinationSelected;
        }

        private void OnCombinationSelected(PlayerController.CardPattern pattern)
        {
            View.SetCombinationText(pattern.PatternType);
        }
    }
}
