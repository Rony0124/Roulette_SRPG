using TMPro;
using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.Bootstrap
{
    public class BootstrapView : ViewBase
    {
        private enum BootstrapButton
        {
            OptionButton,
            SkillButton,
            JokerButton,
            ArtifactButton
        }
        
        private enum BootstrapTransform
        {
            ArtifactSetting
        }

        private enum BootstrapText
        {
            QuantityText,
            GoldTxt
        }
        
        public Button optionButton { get; private set; }
        public Button skillButton{ get; private set; }
        public Button jokerButton{ get; private set; }
        public Button artifactButton { get; private set; }
        public Transform artifactsContainer{ get; private set; }
        
        private TextMeshProUGUI quantityText;
        
        private TextMeshProUGUI goldText;
        
        private void Awake()
        {
            Bind<Button>(typeof(BootstrapButton));
            Bind<Transform>(typeof(BootstrapTransform));
            Bind<TextMeshProUGUI>(typeof(BootstrapText));
            
            optionButton = Get<Button>((int)BootstrapButton.OptionButton);
            skillButton = Get<Button>((int)BootstrapButton.SkillButton);
            jokerButton = Get<Button>((int)BootstrapButton.JokerButton);
            artifactButton = Get<Button>((int)BootstrapButton.ArtifactButton);
            artifactsContainer = Get<Transform>((int)BootstrapTransform.ArtifactSetting);
            quantityText = Get<TextMeshProUGUI>((int)BootstrapText.QuantityText);
            goldText = Get<TextMeshProUGUI>((int)BootstrapText.GoldTxt);
        }
        
        public void SetQuantityText(int quantity, int maxSlot)
        {
            quantityText.text = quantity + "/"+ maxSlot;
        }

        public void SetGoldText(int gold)
        {
            goldText.text = gold.ToString();
        }
    }
}
