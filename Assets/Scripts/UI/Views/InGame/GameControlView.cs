using TSoft.InGame;
using TSoft.InGame.Player;
using TSoft.UI.Core;
using UnityEngine;

namespace TSoft.UI.Views.InGame
{
    public class GameControlView : ViewBase
    {
        private enum GameControlText
        {
            CombinationNameText,
            GoldTxt
        }
        
        private TMPro.TextMeshProUGUI txtCombinationNameText;
        private TMPro.TextMeshProUGUI txtGoldTxt;

        [SerializeField] private PlayerController player;
        
        private void Start()
        {
            Bind<TMPro.TextMeshProUGUI>(typeof(GameControlText));
            
            txtCombinationNameText = Get<TMPro.TextMeshProUGUI>((int)GameControlText.CombinationNameText);
            txtGoldTxt = Get<TMPro.TextMeshProUGUI>((int)GameControlText.GoldTxt);

            UpdateGold(GameSave.Instance.Gold);
        }

        protected override void OnActivated()
        {
            player.onPatternSelected += UpdateCombination;
        }

        protected override void OnDeactivated()
        {
            player.onPatternSelected -= UpdateCombination;
        }

        public void UpdateCombination(PlayerController.CardPattern pattern)
        {
            txtCombinationNameText.text = pattern.PatternType.ToString();
        }

        public void UpdateGold(int gold)
        {
            txtGoldTxt.text = gold.ToString();
        }
    }
}
