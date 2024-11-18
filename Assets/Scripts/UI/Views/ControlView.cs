using System.Collections.Generic;
using TSoft.InGame.CardSystem;
using TSoft.Managers;
using TSoft.UI.Core;
using UnityEngine;

namespace TSoft.UI.Views
{
    public class ControlView : ViewBase
    {
        [SerializeField] private CardViewer cardPrefab;
        [SerializeField] private CardsHolder cardHolder;
        [SerializeField] private TMPro.TextMeshProUGUI txtEnergy;
        
        private int cardIdx = 1;

        private void Start()
        {
            UpdateEnergy();
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
            UpdateEnergy();
        }
        private void UpdateEnergy()
        {
            txtEnergy.text = cardHolder.GetEnergy() + "/" + cardHolder.energy;
        }

        public void OnDrawCard()
        {
            CardViewer card = Instantiate(cardPrefab);
            card.name = "Card " + cardIdx;

            var cardData = CreateRandomCard();
            if(cardData == null)
                return;
            
            card.SetData(cardData);

            cardHolder.AddCard(card);
            cardIdx++;
        }
        public void OnDraw5Cards()
        {
            for (int i = 0; i < 5; i++)
            {
                OnDrawCard();
            }
        }

        public void DiscardHand()
        {
            cardHolder.DiscardAll();
        }

        private CardData CreateRandomCard()
        {
            return CardManager.Instance.TryDrawCard(out var card) ? card.Data.Clone() : null;
        }

        public void AddEnergy()
        {
            cardHolder.AddEnergy(1);
            UpdateEnergy();
        }
    }
}
