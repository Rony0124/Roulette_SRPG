using TSoft.InGame.CardSystem;
using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        
        [SerializeField] private PokerCard pokerCardPrefab;
        [SerializeField] private CardsHolder cardHolder;
        
        private TMPro.TextMeshProUGUI txtEnergy;
        private TMPro.TextMeshProUGUI txtHeart;
        
        private int cardIdx = 1;

        private void Start()
        {
            Bind<Button>(typeof(ControlButton));
            Bind<TMPro.TextMeshProUGUI>(typeof(ControlText));
            
            Get<Button>((int)ControlButton.ButtonDiscard).gameObject.BindEvent(OnDiscardCard);
            Get<Button>((int)ControlButton.ButtonUse).gameObject.BindEvent(OnUseCard);

            txtEnergy = Get<TMPro.TextMeshProUGUI>((int)ControlText.EnergyAmount);
            txtHeart = Get<TMPro.TextMeshProUGUI>((int)ControlText.HeartAmount);
            
            UpdateEnergy();
            UpdateHeart();
            DrawCards();
        }

        protected override void OnActivated()
        {
            CardsHolder.OnCardUsed += OnCardUsed;
        }

        protected override void OnDeactivated()
        {
            CardsHolder.OnCardUsed -= OnCardUsed;
        }

        private void OnCardUsed(CardData card)
        {
            
        }
        
        private void OnDiscardCard(PointerEventData data)
        {
            if(!cardHolder.TryDiscardSelectedCard())
                return;
            
            UpdateEnergy();
            DrawCards();
        }

        private void OnUseCard(PointerEventData data)
        {
            if (!cardHolder.TryUseCardsOnHand()) 
                return;
            
            UpdateHeart();
            DrawCards();
        }
        
        private void DrawCards()
        {
            var cardVoids = cardHolder.CurrentCapacity - cardHolder.CardsOnHand.Count;
            Debug.Log($"current card capacity : {cardHolder.CurrentCapacity}");
            Debug.Log($"current remaining card capacity : {cardVoids}");

            if (cardVoids < 1)
            {
                Debug.Log($"no space on hand left!");
                return;
            }
            
            for (var i = 0; i < cardVoids; i++)
            {
                PokerCard pokerCard = Instantiate(pokerCardPrefab);
                pokerCard.name = "Card " + cardIdx;

                var cardData = CreateRandomCard();
                if(cardData == null)
                    return;
            
                pokerCard.SetData(cardData);  

                cardHolder.AddCard(pokerCard);
                cardIdx++;
            }
        }
        
        private CardData CreateRandomCard()
        {
            return cardHolder.TryDrawCard(out var card) ? card.Data.Clone() : null;
        }
        
        private void UpdateEnergy()
        {
            txtEnergy.text = cardHolder.CurrentEnergy + "";
        }
        
        private void UpdateHeart()
        {
            txtHeart.text = cardHolder.CurrentHeart + "";
        }
    }
}
