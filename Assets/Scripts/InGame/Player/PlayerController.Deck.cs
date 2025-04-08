using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TCGStarter.Tweening;
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
        
          private void DiscardSelectedCards()
        {
            foreach (var card in currentPokerCardSelected)
            {
                card.Dissolve(animationSpeed);
                
                Discard(card);
            }
            
            currentPokerCardSelected.Clear();
        }

        public void Discard(PokerCard pokerCard)
        {
            pokerCard.ClearEvents();
            RemoveCardFromHand(pokerCard);
            
            pokerCard.PositionCard(0, 0, animationSpeed);
            pokerCard.Discard(animationSpeed);
            
            Destroy(pokerCard.gameObject, 3);
        }
        
        private void DiscardAll()
        {
            List<PokerCard> cards = new(cardsOnHand);
            foreach (var cardOnHand in cards)
            {
                if(cardOnHand == null)
                    continue;
                
                Discard(cardOnHand);
            }
            
            cardsOnHand.Clear();
        }

        public void RetrieveAllCards()
        {
            List<PokerCard> cards = new(cardsOnHand);
            foreach (var cardOnHand in cards)
            {
                if(cardOnHand == null)
                    continue;
                
                cardsOnDeck.Enqueue(cardOnHand.cardData);
                
                Discard(cardOnHand);
            }
            
            cardsOnHand.Clear();
        }
        
        private void RemoveCardFromHand(PokerCard pokerCard)
        {
            cardsOnHand.Remove(pokerCard);
            ArrangeHand(animationSpeed / 2f);
        }
        
        public void AddCard(PokerCard pokerCard)
        {
            cardsOnHand.Add(pokerCard);
            
            pokerCard.gameObject.transform.SetParent(hand);
            pokerCard.transform.localPosition = deck.transform.localPosition;
            pokerCard.transform.localScale = Vector3.one;

            ArrangeHand(animationSpeed);
            StartCoroutine(ListenCardEvents(pokerCard));
        }

        private void ArrangeHand(float duration)
        {
            cardPositions = new Vector3[cardsOnHand.Count];
            cardRotations = new Vector3[cardsOnHand.Count];
            float xspace = cardXSpacing / 2;
            float yspace = 0;
            float angle = cardAngle;
            int mid = cardsOnHand.Count / 2;

            if (cardsOnHand.Count % 2 == 1)
            {
                cardPositions[mid] = new Vector3(0, 0, 0);
                cardRotations[mid] = new Vector3(0, 0, 0);

                RelocateCard(cardsOnHand[mid], 0, 0, 0, duration);
                mid++;
                xspace = cardXSpacing;
                yspace = -cardYSpacing;
            }

            for (int i = mid; i < cardsOnHand.Count; i++)
            {
                cardPositions[i] = new Vector3(xspace, yspace, 0);
                cardRotations[i] = new Vector3(0, 0, -angle);
                cardPositions[cardsOnHand.Count - i - 1] = new Vector3(-xspace, yspace, 0);
                cardRotations[cardsOnHand.Count - i - 1] = new Vector3(0, 0, angle);

                RelocateCard(cardsOnHand[i], xspace, yspace, -angle, duration);
                RelocateCard(cardsOnHand[cardsOnHand.Count - i - 1], -xspace, yspace, angle, duration);
                
                cardsOnHand[cardsOnHand.Count - i - 1].SetCardOrder(cardsOnHand.Count - i - 1);
                cardsOnHand[i].SetCardOrder(i);

                xspace += cardXSpacing;
                yspace -= cardYSpacing;
                yspace *= 1.5f;
                angle += cardAngle;
            }
        }
        
        private void RelocateCard(PokerCard card, float x, float y, float angle, float duration)
        {
            PositionCard(card, x, y, duration);
            RotateCard(card, angle, duration);
        }
        
        private void PositionCard(PokerCard card, float x, float y, float duration)
        {
            card.transform.TweenMove(new Vector3(x, cardY + y, 0), duration);
        }
        private void RotateCard(PokerCard card, float angle, float duration)
        {
            card.transform.TweenRotate(new Vector3(0, 0, angle), duration);
        }
    }
}
