using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TSoft.Data.Card;
using TSoft.Data.Registry;
using TSoft.InGame.CardSystem;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController
    {
        public Action<int, int> onDeckChanged; 
        
        [Title("Deck")]
        [SerializeField] private List<CardInfo> defaultCardDB;
        [SerializeField] private List<CardInfo> testCardDB;
        
        [Header("Game Object")]
        [SerializeField] private PokerCard pokerCardPrefab;
        
        private Queue<CardInfo> cardsOnDeck;
        
        public int maxCardsNum; 

        private void InitializeDeck()
        {
            List<CardInfo> currentDeck = new();
            cardsOnDeck = new();
            
            foreach (var card in defaultCardDB)
            {
                currentDeck.Add(card);
            }
            
            foreach (var kvp in DataRegistry.Instance.JokerRegistry.assetGuidLookup
                         .Where(kvp => GameSave.Instance.HasItemsId(kvp.Key)))
            {
                currentDeck.Add(kvp.Value);
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
        
        private CardInfo CreateRandomCard()
        {
            return TryDrawCard(out var card) ? card : null;
        }

        private bool TryDrawCard(out CardInfo card)
        {
            card = null;
            
            if (cardsOnDeck.Count < 1)
                return false;

            card = cardsOnDeck.Dequeue();
            
            onDeckChanged?.Invoke(cardsOnDeck.Count, maxCardsNum);
            
            return true;
        }

        public void Shuffle(List<CardInfo> cards)
        {
            cards.ShuffleList();
            
#if UNITY_EDITOR
            foreach (var card in testCardDB)
            {
                cardsOnDeck.Enqueue(card);
            }
#endif
            
            foreach (var card in cards)
            {
                cardsOnDeck.Enqueue(card);
            }
        }

        public void ShuffleCurrent()
        {
            cardsOnDeck.ToList().ShuffleList();
        }
    }
}
