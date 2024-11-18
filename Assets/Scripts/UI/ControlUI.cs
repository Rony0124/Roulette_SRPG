using System.Collections.Generic;
using InGame.CardSystem;
using TSoft.InGame.CardSystem;
using UnityEngine;

namespace TSoft.UI
{
    public class ControlUI : MonoBehaviour
    {
        [SerializeField] private CardViewer cardPrefab;
        [SerializeField] private CardsHolder cardHolder;
        [SerializeField] private TMPro.TextMeshProUGUI txtEnergy;

        [SerializeField] List<CardSO> cardsDB;

        // used just for naming the cards gameobjects.
        private int cardIdx = 1;

        private void Start()
        {
            UpdateEnergy();
        }
        private void OnEnable()
        {
            CardsHolder.OnCardUsed += OnCardUsed;
        }

        private void OnDisable()
        {
            CardsHolder.OnCardUsed -= OnCardUsed;
        }

        private void OnCardUsed(CardData card)
        {
            UpdateEnergy();
        }
        private void UpdateEnergy()
        {
            txtEnergy.text = cardHolder.GetEnergy().ToString() + "/" + CardsHolder.MAX_ENERGY;
        }

        public void OnDrawCard()
        {

            CardViewer card = Instantiate(cardPrefab);
            card.name = "Card " + cardIdx;
            card.SetData(CreateRandomCard());

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
            int idx = Random.Range(0, cardsDB.Count);

            return cardsDB[idx].Data.Clone();
        }

        public void AddEnergy()
        {
            cardHolder.AddEnergy(1);
            UpdateEnergy();
        }
    }
}
