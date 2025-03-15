using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.InGame
{
    public class GameplayView : ViewBase
    {
        private enum GameplayText
        {
            EnergyAmount,
            HeartAmount,
            DeckText
        }
        
        private enum GameplayButton
        {
            ButtonDiscard,
            ButtonUse,
        }
        
        private enum GameplayTransform
        {
            JokerStackPanel
        }
   
        //UI
        private TMPro.TextMeshProUGUI txtEnergy;
        private TMPro.TextMeshProUGUI txtHeart;
        private TMPro.TextMeshProUGUI txtDeck;

        public Button DiscardButton { get; private set; }
        public Button UseButton { get; private set; }
        public Transform JokerEffectParent { get; private set; }
        
        private void Awake()
        {
            Bind<Button>(typeof(GameplayButton));
            Bind<TMPro.TextMeshProUGUI>(typeof(GameplayText));
            Bind<Transform>(typeof(GameplayTransform));
            
            DiscardButton = Get<Button>((int)GameplayButton.ButtonDiscard);
            UseButton = Get<Button>((int)GameplayButton.ButtonUse);
            
            txtEnergy = Get<TMPro.TextMeshProUGUI>((int)GameplayText.EnergyAmount);
            txtHeart = Get<TMPro.TextMeshProUGUI>((int)GameplayText.HeartAmount);
            txtDeck = Get<TMPro.TextMeshProUGUI>((int)GameplayText.DeckText);
            
            JokerEffectParent = Get<Transform>((int)GameplayTransform.JokerStackPanel);
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
