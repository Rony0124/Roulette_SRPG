using System.Collections.Generic;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public partial class CardsHolder
    {
        [Header("Deck")]
        [SerializeField] private List<CardSO> defaultCardDB;

        private Queue<CardSO> cardsOnDeck;

        private void InitializeDeck()
        {
            List<CardSO> currentDeck = new();
            cardsOnDeck = new();
            
            foreach (var card in defaultCardDB)
            {
                currentDeck.Add(card);
            }

            currentDeck.ShuffleList();
            cardsOnDeck = new Queue<CardSO>(currentDeck);
        }

        public bool TryDrawCard(out CardSO card)
        {
            card = null;
            
            if (cardsOnDeck.Count < 1)
                return false;

            card = cardsOnDeck.Dequeue();
            
            Debug.Log($"current remaining card on deck : {cardsOnDeck.Count}");
            
            return true;
        }

        public void ResetDeck()
        {
            cardsOnDeck = new();
        }
    }
}