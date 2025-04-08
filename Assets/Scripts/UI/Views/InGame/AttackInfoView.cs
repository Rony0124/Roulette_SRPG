using TSoft.InGame;
using TSoft.UI.Core;
using UnityEngine;

namespace TSoft.UI.Views.InGame
{
    public class AttackInfoView : ViewBase
    {
        private enum AttackInfoText
        {
            CombinationNameText,
            BasicDmgText
        }

        private enum AttackInfoTransform
        {
            TextPool
        }
        
        public Transform textPoolPrent { get; private set; }
        
        private TMPro.TextMeshProUGUI txtCombinationNameText;
        
        private void Start()
        {
            Bind<TMPro.TextMeshProUGUI>(typeof(AttackInfoText));
            Bind<Transform>(typeof(AttackInfoTransform));
            
            txtCombinationNameText = Get<TMPro.TextMeshProUGUI>((int)AttackInfoText.CombinationNameText);
            textPoolPrent = Get<Transform>((int)AttackInfoTransform.TextPool);
        }

        public void SetCombinationText(CardPatternType patternType)
        {
            txtCombinationNameText.text = patternType.ToString();
        }
    }
}
