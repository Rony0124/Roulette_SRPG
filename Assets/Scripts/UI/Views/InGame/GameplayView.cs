using TSoft.UI.Core;
using UnityEngine.UI;

namespace TSoft.UI.Views.InGame
{
    public class GameplayView : ViewBase
    {
        private enum ControlText
        {
            EnergyAmount,
            HeartAmount,
            DeckText
        }
        
        private enum ControlButton
        {
            ButtonDiscard,
            ButtonUse,
        }
   
        //UI
        private TMPro.TextMeshProUGUI txtEnergy;
        private TMPro.TextMeshProUGUI txtHeart;
        private TMPro.TextMeshProUGUI txtDeck;

        public Button DiscardButton { get; private set; }
        public Button UseButton { get; private set; }
        
        private void Awake()
        {
            Bind<Button>(typeof(ControlButton));
            Bind<TMPro.TextMeshProUGUI>(typeof(ControlText));
            
            DiscardButton = Get<Button>((int)ControlButton.ButtonDiscard);
            UseButton = Get<Button>((int)ControlButton.ButtonUse);
            
            txtEnergy = Get<TMPro.TextMeshProUGUI>((int)ControlText.EnergyAmount);
            txtHeart = Get<TMPro.TextMeshProUGUI>((int)ControlText.HeartAmount);
            txtDeck = Get<TMPro.TextMeshProUGUI>((int)ControlText.DeckText);
        }

        public void SetHeartText(float currentHeart, float maxHeart)
        {
            txtHeart.text = currentHeart + " / " + maxHeart;
        }
        
        public void SetEnergyText(float currentEnergy, float maxEnergy)
        {
            txtEnergy.text = currentEnergy + " / " + maxEnergy;
        }

        public void SetDeckText(int cardNum, int maxCardNum)
        {
            txtDeck.text = cardNum + "/" + maxCardNum;
        }
    }
}
