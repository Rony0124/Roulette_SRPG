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
            EnergyAmount
        }
        
        private enum ControlButton
        {
            ButtonDraw,
            ButtonDiscard,
        }
        
        [SerializeField] private PokerCard pokerCardPrefab;
        [SerializeField] private CardsHolder cardHolder;
        private TMPro.TextMeshProUGUI txtEnergy;
        
        private int cardIdx = 1;

        private void Start()
        {
            Bind<Button>(typeof(ControlButton));
            Bind<TMPro.TextMeshProUGUI>(typeof(ControlText));
            
            Get<Button>((int)ControlButton.ButtonDraw).gameObject.BindEvent(OnDrawCard);
            Get<Button>((int)ControlButton.ButtonDiscard).gameObject.BindEvent(OnDiscardCard);

            txtEnergy = Get<TMPro.TextMeshProUGUI>((int)ControlText.EnergyAmount);
            
            UpdateEnergy();
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
        
        private void UpdateEnergy()
        {
            txtEnergy.text = cardHolder.CurrentEnergy + "/" + CardsHolder.DefaultEnergy;
        }

        private void OnDrawCard(PointerEventData data)
        {
            DrawCards();
        }

        private void DrawCards()
        {
            var cardVoids = CardsHolder.DefaultCapacity - cardHolder.CardsOnHand.Count;
            Debug.Log($"current card capacity : {CardsHolder.DefaultCapacity}");
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
        
        private void OnDiscardCard(PointerEventData data)
        {
            cardHolder.DiscardSelectedCard();
            UpdateEnergy();
        }

        private void OnHandCardOnHold()
        {
            cardHolder.UseCardsOnHand();
        }

        private CardData CreateRandomCard()
        {
            return cardHolder.TryDrawCard(out var card) ? card.Data.Clone() : null;
        }
    }
}
