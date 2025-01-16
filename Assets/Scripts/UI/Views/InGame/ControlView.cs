using System;
using TSoft.InGame;
using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine;
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
    
        //Play
        [SerializeField]
        private PlayerController player;
        
        private void Awake()
        {
            Bind<Button>(typeof(ControlButton));
            Bind<TMPro.TextMeshProUGUI>(typeof(ControlText));
            
            Get<Button>((int)ControlButton.ButtonDiscard).gameObject.BindEvent(OnDiscardCard);
            Get<Button>((int)ControlButton.ButtonUse).gameObject.BindEvent(OnUseCard);

            txtEnergy = Get<TMPro.TextMeshProUGUI>((int)ControlText.EnergyAmount);
            txtHeart = Get<TMPro.TextMeshProUGUI>((int)ControlText.HeartAmount);
            txtDeck = Get<TMPro.TextMeshProUGUI>((int)ControlText.DeckText);
        }

        protected override void OnActivated()
        {
            player.onGameReady += UpdateCardOnGameReady;
            player.Gameplay.GetAttrVar(GameplayAttr.Heart).OnValueChanged += OnPlayerHeartChanged;
            player.Gameplay.GetAttrVar(GameplayAttr.Energy).OnValueChanged += OnPlayerEnergyChanged;

            player.onDeckChanged += UpdateDeck;
        }

        protected override void OnDeactivated()
        {
            player.onGameReady -= UpdateCardOnGameReady;
            player.Gameplay.GetAttrVar(GameplayAttr.Heart).OnValueChanged -= OnPlayerHeartChanged;
            player.Gameplay.GetAttrVar(GameplayAttr.Energy).OnValueChanged -= OnPlayerEnergyChanged;
            
            player.onDeckChanged -= UpdateDeck;
        }

        private void UpdateCardOnGameReady()
        {
            var maxEnergyCount = player.Gameplay.GetAttr(GameplayAttr.MaxEnergy);
            var energyCount = player.Gameplay.GetAttr(GameplayAttr.Energy);
            var maxHeartCount = player.Gameplay.GetAttr(GameplayAttr.MaxHeart);
            var heartCount = player.Gameplay.GetAttr(GameplayAttr.Heart);
            
            UpdateEnergy(energyCount, maxEnergyCount);
            UpdateHeart(heartCount, maxHeartCount);
            
            player.DrawCards();
        }
        
        private void OnDiscardCard(PointerEventData data)
        {
            if(!player.TryDiscardSelectedCard())
                return;
            
            player.DrawCards();
        }

        private void OnUseCard(PointerEventData data)
        {
            if (!player.TryUseCardsOnHand()) 
                return;
            
            player.DrawCards();
        }

        private void OnPlayerHeartChanged(float oldVal, float newVal)
        {
            var maxCount = player.Gameplay.GetAttr(GameplayAttr.MaxHeart);
            txtHeart.text = newVal + " / " + maxCount;
        }
        
        private void OnPlayerEnergyChanged(float oldVal, float newVal)
        {
            var maxCount = player.Gameplay.GetAttr(GameplayAttr.MaxEnergy);
            txtEnergy.text = newVal + " / " + maxCount;
        }
        
        private void UpdateEnergy(float curVal, float maxVal)
        {
            txtEnergy.text = curVal + " / " + maxVal;
        }
        
        private void UpdateHeart(float curVal, float maxVal)
        {
            txtHeart.text = curVal + " / " + maxVal;
        }

        private void UpdateDeck(int cardNum, int maxCardNum)
        {
            txtDeck.text = cardNum + "/" + maxCardNum;
        }
    }
}
