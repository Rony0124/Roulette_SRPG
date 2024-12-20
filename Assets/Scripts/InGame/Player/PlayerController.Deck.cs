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
        [Header("Deck")]
        [SerializeField] private List<CardSO> defaultCardDB;
        //TEST
        [SerializeField] private List<CardSO> specialCardDB;

        [Header("Game Object")]
        [SerializeField] private PokerCard pokerCardPrefab;
        
        private Queue<CardSO> cardsOnDeck;

        private void InitializeDeck()
        {
            List<CardSO> currentDeck = new();
            cardsOnDeck = new();
            
            foreach (var card in defaultCardDB)
            {
                currentDeck.Add(card);
            }
            
            Shuffle(currentDeck);
        }
        
        public void DrawCards()
        {
            var cardVoids = Gameplay.GetAttr(GameplayAttr.Capacity) - CardsOnHand.Count;
            
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
            
            return true;
        }

        public void Shuffle(List<CardSO> cards)
        {
            cards.ShuffleList();
      
            //test 가장 마지막에 올린다
            foreach (var card in specialCardDB)
            {
                cardsOnDeck.Enqueue(card);
            }

            foreach (var card in cards)
            {
                cardsOnDeck.Enqueue(card);
            }
            
            //  cardsOnDeck = new Queue<CardSO>(cards);
        }

        public void ShuffleCurrent()
        {
            cardsOnDeck.ToList().ShuffleList();
        }
    }
}
