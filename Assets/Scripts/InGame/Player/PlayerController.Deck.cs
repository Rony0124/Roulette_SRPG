using System;
using System.Collections.Generic;
using System.Linq;
using TSoft.Data.Card;
using TSoft.InGame.CardSystem;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController
    {
        public Action<int, int> onDeckChanged; 
        
        [Header("Deck")]
        [SerializeField] private List<CardSO> defaultCardDB;
        [SerializeField] private List<CardSO> specialCardDB;

        [Header("Game Object")]
        [SerializeField] private PokerCard pokerCardPrefab;
        
        private Queue<CardSO> cardsOnDeck;
        
        public int maxCardsNum; 

        private void InitializeDeck()
        {
            List<CardSO> currentDeck = new();
            cardsOnDeck = new();
            
            foreach (var card in defaultCardDB)
            {
                currentDeck.Add(card);
            }
            
            Shuffle(currentDeck);

            maxCardsNum = cardsOnDeck.Count;
        }
        
        public void DrawCards()
        {
            var cardVoids = Gameplay.GetAttr(GameplayAttr.Capacity) - cardsOnHand.Count;
            
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

                AddCard(pokerCard);
            }
        }
        
        private CardSO CreateRandomCard()
        {
            return TryDrawCard(out var card) ? card : null;
        }

        private bool TryDrawCard(out CardSO card)
        {
            card = null;
            
            if (cardsOnDeck.Count < 1)
                return false;

            card = cardsOnDeck.Dequeue();
            
            onDeckChanged?.Invoke(cardsOnDeck.Count, maxCardsNum);
            
            return true;
        }

        public void Shuffle(List<CardSO> cards)
        {
            cards.ShuffleList();

            foreach (var card in cards)
            {
                cardsOnDeck.Enqueue(card);
            }
        }

        public void ShuffleCurrent()
        {
            cardsOnDeck.ToList().ShuffleList();
        }

        public void AddJoker(CardSO specialCard)
        {
            specialCardDB.Add(specialCard);
        }
        
        public void RemoveJoker(CardSO specialCard)
        {
            specialCardDB.Remove(specialCard);
        }
    }
}
