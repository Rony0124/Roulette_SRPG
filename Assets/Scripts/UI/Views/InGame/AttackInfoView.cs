using TSoft.InGame;
using TSoft.UI.Core;

namespace TSoft.UI.Views.InGame
{
    public class AttackInfoView : ViewBase
    {
        private enum GameControlText
        {
            CombinationNameText
        }
        
        private TMPro.TextMeshProUGUI txtCombinationNameText;
        
        private void Start()
        {
            Bind<TMPro.TextMeshProUGUI>(typeof(GameControlText));
            
            txtCombinationNameText = Get<TMPro.TextMeshProUGUI>((int)GameControlText.CombinationNameText);
        }

        public void SetCombinationText(CardPatternType patternType)
        {
            txtCombinationNameText.text = patternType.ToString();
        }
    }
}
