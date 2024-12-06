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
        
        private enum ControlParent
        {
            HeartGroup,
            EnergyGroup
        }
        
        [Header("Game Object")]
        [SerializeField] private PokerCard pokerCardPrefab;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private GameObject energyPrefab;
        
        //UI
        private TMPro.TextMeshProUGUI txtEnergy;
        private TMPro.TextMeshProUGUI txtHeart;
        private Transform trHeartGroup;
        private Transform trEnergyGroup;
        //Play
        private PlayerController player;
        private InGameDirector director;

        private List<GameObject> hearts;
        private List<GameObject> energies;
        
        private void Start()
        {
            Bind<Button>(typeof(ControlButton));
            Bind<TMPro.TextMeshProUGUI>(typeof(ControlText));
            Bind<Transform>(typeof(ControlParent));
            
            Get<Button>((int)ControlButton.ButtonDiscard).gameObject.BindEvent(OnDiscardCard);
            Get<Button>((int)ControlButton.ButtonUse).gameObject.BindEvent(OnUseCard);

            txtEnergy = Get<TMPro.TextMeshProUGUI>((int)ControlText.EnergyAmount);
            txtHeart = Get<TMPro.TextMeshProUGUI>((int)ControlText.HeartAmount);
            trHeartGroup = Get<Transform>((int)ControlParent.HeartGroup);
            trEnergyGroup = Get<Transform>((int)ControlParent.EnergyGroup);

            player = FindObjectOfType<PlayerController>();
            
            hearts = new();
            energies = new();
        }

        protected override void OnActivated()
        {
            //director 참조 타이밍 개선 필요
            if (director == null)
            {
                director = GameContext.Instance.CurrentDirector as InGameDirector;
                if (director == null)
                {
                    director = FindObjectOfType<InGameDirector>();
                }
            }

            if (director != null)
            {
                director.OnPrePlay += UpdateCardOnPrePlay;
            }
        }

        protected override void OnDeactivated()
        {
            if (director != null)
            {
                director.OnPrePlay -= UpdateCardOnPrePlay;
            }
        }

        private void UpdateCardOnPrePlay()
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
            Debug.Log($"current remaining card capacity : {cardVoids}");

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

            if (energies.Count > 0)
            {
                for (int i = 0; i < energies.Count; i++)
                {
                    Destroy(energies[i]);
                }
            
                energies.Clear();    
            }

            for (int i = 0; i < energyCount; i++)
            {
                var obj = Instantiate(energyPrefab, trEnergyGroup);
                energies.Add(obj);
            }
        }
        
        private void UpdateHeart()
        {
            var heartCount = player.Gameplay.GetAttr(GameplayAttr.Heart);
            txtHeart.text = heartCount + "";

            if (hearts.Count > 0)
            {
                for (int i = 0; i < hearts.Count; i++)
                {
                    Destroy(hearts[i]);
                }
            
                hearts.Clear();
            }
            
            for (int i = 0; i < heartCount; i++)
            {
                var obj = Instantiate(heartPrefab, trHeartGroup);
                hearts.Add(obj);
            }
        }
    }
}
