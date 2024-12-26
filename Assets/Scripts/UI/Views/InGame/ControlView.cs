using TSoft.InGame;
using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.UI.Views.InGame
{
    public class ControlView : ViewBase
    {
        private enum ControlText
        {
            EnergyAmount,
            HeartAmount
        }
        
        private enum ControlButton
        {
            ButtonDiscard,
            ButtonUse,
        }
   
        //UI
        private TMPro.TextMeshProUGUI txtEnergy;
        private TMPro.TextMeshProUGUI txtHeart;
    
        //Play
        private PlayerController player;
        
        private void Awake()
        {
            Bind<Button>(typeof(ControlButton));
            Bind<TMPro.TextMeshProUGUI>(typeof(ControlText));
            
            Get<Button>((int)ControlButton.ButtonDiscard).gameObject.BindEvent(OnDiscardCard);
            Get<Button>((int)ControlButton.ButtonUse).gameObject.BindEvent(OnUseCard);

            txtEnergy = Get<TMPro.TextMeshProUGUI>((int)ControlText.EnergyAmount);
            txtHeart = Get<TMPro.TextMeshProUGUI>((int)ControlText.HeartAmount);

            player = FindObjectOfType<PlayerController>();
        }

        protected override void OnActivated()
        {
            //director 참조 타이밍 개선 필요
            if (player == null)
            {
                player = FindObjectOfType<PlayerController>();
            }

            if (player != null)
            {
                player.onGameReady += UpdateCardOnGameReady;
            }
        }

        protected override void OnDeactivated()
        {
            if (player != null)
            {
                player.onGameReady -= UpdateCardOnGameReady;
            }
        }

        private void UpdateCardOnGameReady()
        {
            UpdateEnergy();
            UpdateHeart();
            
            player.DrawCards();
        }
        
        private void OnDiscardCard(PointerEventData data)
        {
            if(!player.TryDiscardSelectedCard())
                return;
            
            UpdateEnergy();
            player.DrawCards();
        }

        private void OnUseCard(PointerEventData data)
        {
            if (!player.TryUseCardsOnHand()) 
                return;
            
            UpdateHeart();
            
            player.DrawCards();
        }
        
        private void UpdateEnergy()
        {
            var energyCount = player.Gameplay.GetAttr(GameplayAttr.Energy);
            txtHeart.text = energyCount + "";
        }
        
        private void UpdateHeart()
        {
            var heartCount = player.Gameplay.GetAttr(GameplayAttr.Heart);
            txtHeart.text = heartCount + "";
        }
    }
}
