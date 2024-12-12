using System;
using System.Collections.Generic;
using TSoft.InGame;
using TSoft.InGame.CardSystem;
using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.UI.Views
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
        
        [Header("Game Object")]
        [SerializeField] private PokerCard pokerCardPrefab;
   
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
            DrawCards();
        }
        
        private void OnDiscardCard(PointerEventData data)
        {
            if(!player.TryDiscardSelectedCard())
                return;
            
            UpdateEnergy();
            DrawCards();
        }

        private void OnUseCard(PointerEventData data)
        {
            if (!player.TryUseCardsOnHand()) 
                return;
            
            UpdateHeart();
            DrawCards();
        }
        
        private void DrawCards()
        {
            var cardVoids = player.Gameplay.GetAttr(GameplayAttr.Capacity) - player.CardsOnHand.Count;
            
            if (cardVoids < 1)
            {
                Debug.Log($"no space on hand left!");
                return;
            }
            
            for (var i = 0; i < cardVoids; i++)
            {
                PokerCard pokerCard = Instantiate(pokerCardPrefab);

                var cardData = CreateRandomCard();
                if(cardData == null)
                    return;
            
                pokerCard.SetData(cardData);  

                player.AddCard(pokerCard);
            }
        }
        
        private CardData CreateRandomCard()
        {
            return player.TryDrawCard(out var card) ? card.Data.Clone() : null;
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
