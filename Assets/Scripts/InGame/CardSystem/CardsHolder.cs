using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TSoft.InGame.GamePlaySystem;
using TSoft.Managers;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public partial class CardsHolder : MonoBehaviour
    {
        public static event Action<CardData> OnCardUsed;
        
        [Header("Positions")]
        [SerializeField] private Transform hand;
        [SerializeField] private Transform cardPreview;
        [SerializeField] private Transform deck;
        
        [Header("GamePlay")]
        private Gameplay gameplay;
        
        //animation
        private Vector3[] cardPositions;
        private int currentCardPreviewIdx;
        
        private List<PokerCard> cardsOnHand;
        private List<PokerCard> currentPokerCardSelected;
        
        private PokerCard currentPokerCardPreview;
        private PokerCard currentPokerCardHold;
       
        public List<PokerCard> CardsOnHand => cardsOnHand;
        public Gameplay Gameplay =>  gameplay;
        
        private const int HandCountMax = 5;
        
        private void Awake()
        {
            currentPokerCardSelected = new List<PokerCard>();
            cardsOnHand = new List<PokerCard>();

            gameplay = GetComponent<Gameplay>();
            
            InitializeDeck();
        }
        
        public bool TryUseCardsOnHand()
        {
            var currentHeart = gameplay.GetAttr(GameplayAttr.Heart);
            if (currentHeart <= 0)
                return false;

            var damage = 0;
            foreach (var selectedCard in currentPokerCardSelected)
            {
                OnCardUsed?.Invoke(selectedCard.cardData);

                damage += selectedCard.cardData.Damage;
            
                selectedCard.Dissolve(animationSpeed);
                
                Discard(selectedCard);
            }

            CombatManager.Instance.Combat(damage);

            --currentHeart;
            gameplay.SetAttr(GameplayAttr.Heart, currentHeart);
            currentPokerCardSelected = new List<PokerCard>();
            
            return true;
        }
        
        public bool TryDiscardSelectedCard()
        {
            var currentEnergy = gameplay.GetAttr(GameplayAttr.Energy);
            if(currentEnergy <= 0)
                return false;
            
            foreach (var card in currentPokerCardSelected)
            {
                Discard(card);
            }
            
            --currentEnergy;
            gameplay.SetAttr(GameplayAttr.Energy, currentEnergy);
            currentPokerCardSelected = new();
            
            return true;
        }

        private void Discard(PokerCard pokerCard)
        {
            pokerCard.ClearEvents();
            RemoveCardFromHand(pokerCard);
            
            pokerCard.PositionCard(0, 0, animationSpeed);
            pokerCard.Discard(animationSpeed);
            
            Destroy(pokerCard.gameObject, 3);
        }
        
        private void RemoveCardFromHand(PokerCard pokerCard)
        {
            currentPokerCardPreview = null;
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
            
            var xSpacing = cardXSpacing;
            var mid = cardsOnHand.Count / 2;

            if (cardsOnHand.Count % 2 == 1)
            {
                cardPositions[mid] = new Vector3(0, cardYSpacing, 0);
                
                cardsOnHand[mid].PositionCard(0, cardY + cardYSpacing, duration);
                
                mid++;
                xSpacing = cardXSpacing;
            }

            for (var i = mid; i < cardsOnHand.Count; i++)
            {
                if (i == mid)
                {
                    xSpacing /= 2;
                }
                
                cardPositions[i] = new Vector3(xSpacing, cardYSpacing, 0);
                cardPositions[cardsOnHand.Count - i - 1] = new Vector3(-xSpacing, cardYSpacing, 0);
           
                cardsOnHand[i].PositionCard(xSpacing, cardY + cardYSpacing, duration);
                cardsOnHand[cardsOnHand.Count - i - 1].PositionCard(-xSpacing,cardY + cardYSpacing, duration);

                xSpacing += cardXSpacing;
            }
        }

        private void CheckCardPatternOnHand()
        {
            if (currentPokerCardSelected is not { Count: > 0 })
                return;
            
            var rankGroups = currentPokerCardSelected
                .GroupBy(card => card.cardData.Number)
                .OrderByDescending(group => group.Count())
                .ToList();
            
            var suitGroups = currentPokerCardSelected
                .GroupBy(card => card.cardData.Type)
                .OrderByDescending(group => group.Count())
                .ToList();
            
            var sortedRanks = currentPokerCardSelected
                .Select(card => card.cardData.Number)
                .Distinct()
                .OrderBy(rank => rank)
                .ToList();
            
            string detectedPattern = null;

            if (CheckForStraightFlush(currentPokerCardSelected))
            {
                detectedPattern = "Straight Flush";
            }
            else if (rankGroups.Any(g => g.Count() == 4))
            {
                detectedPattern = "Four of a Kind";
            }
            else if (rankGroups.Any(g => g.Count() == 3) && rankGroups.Any(g => g.Count() == 2))
            {
                detectedPattern = "Full House";
            }
            else if (suitGroups.Any(g => g.Count() >= 5))
            {
                detectedPattern = "Flush";
            }
            else if (CheckForStraight(sortedRanks))
            {
                detectedPattern = "Straight";
            }
            else if (rankGroups.Any(g => g.Count() == 3))
            {
                detectedPattern = "Three of a Kind";
            }
            else if (rankGroups.Count(g => g.Count() == 2) >= 2)
            {
                detectedPattern = "Two Pair";
            }
            else if (rankGroups.Any(g => g.Count() == 2))
            {
                detectedPattern = "One Pair";
            }
            else
            {
                detectedPattern = "High Card";
            }

            Debug.Log($"Highest Pattern Detected: {detectedPattern}");
        }
        
        private bool CheckForStraightFlush(List<PokerCard> cards)
        {
            var suitGroups = cards.GroupBy(card => card.cardData.Type);

            foreach (var suitGroup in suitGroups)
            {
                var sortedRanks = suitGroup
                    .Select(card => card.cardData.Number)
                    .Distinct()
                    .OrderBy(rank => rank)
                    .ToList();

                if (CheckForStraight(sortedRanks))
                    return true;
            }

            return false;
        }
        
        private bool CheckForStraight(List<int> sortedRanks)
        {
            int consecutiveCount = 1;
            for (int i = 1; i < sortedRanks.Count; i++)
            {
                if (sortedRanks[i] == sortedRanks[i - 1] + 1)
                {
                    consecutiveCount++;
                    if (consecutiveCount >= 5)
                        return true;
                }
                else
                {
                    consecutiveCount = 1;
                }
            }
            return false;
        }
        

    }
}