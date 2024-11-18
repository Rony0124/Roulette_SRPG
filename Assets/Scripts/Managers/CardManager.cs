using System;
using System.Collections.Generic;
using TSoft.Core;
using TSoft.InGame.CardSystem;
using TSoft.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TSoft.Managers
{
    public class CardManager : Singleton<CardManager>
    {
        [SerializeField] private List<CardSO> defaultCardDB;

        private List<CardSO> currentDeck;
        public List<CardSO> CurrentDeck => currentDeck; 
        
        private Queue<CardSO> cardsOnDeck;
        private List<CardSO> cardsOnHand;
        private List<CardSO> discardedCards;
        
        private void Awake()
        {
            currentDeck = new();
            cardsOnDeck = new();
            cardsOnHand = new();
            discardedCards = new();
            
            InitializeCards();
        }

        private void InitializeCards()
        {
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
            cardsOnHand.Add(card);
            
            return true;
        }
    }
}
