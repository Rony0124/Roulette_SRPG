using System;
using System.Collections;
using System.Collections.Generic;
using TCGStarter;
using TCGStarter.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace TSoft.InGame.CardSystem
{
    public class CardsHolder : MonoBehaviour
    {
        public static event Action<CardData> OnCardUsed;

        [Header("Visuals")]
        [SerializeField] private bool isLiveVisualsUpdate = false;
        [SerializeField] private float cardY = 150;
        [SerializeField] private float cardXSpacing = 200;
        [SerializeField] private float cardYSpacing = 10;
        [Range(0.2f, 2f)]
        [SerializeField] private float animationSpeed = 0.5f;

        [Header("Positions")]
        [SerializeField] private Transform hand;
        [SerializeField] private Transform cardPreview;
        [SerializeField] private Transform deck;
        [SerializeField] private Transform cardHold;

        [Header("Prefabs")]
        [SerializeField] private TargetingArrow arrowPrefab;
        
        private List<CardViewer> cards = new();
        private Vector3[] cardPositions;
        private CardViewer currentCardPreview;
        private CardViewer currentCardHold;
        private int currentCardPreviewIdx;
        private int currentCardHoldIdx = -1;
        private RectTransform holderRect;
        private TargetingArrow currentArrow;

        private float liveUpdateTime = 0;
        private int currentEnergy = 5;
        public int energy;
        
        private void Awake()
        {
            holderRect = GetComponent<RectTransform>();
        }
        
        void Update()
        {
            if (currentCardHold != null)
            {
                if (!currentCardHold.cardData.IsTargetable)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    currentCardHold.transform.position = mousePosition;
                }
            }
            
            if (isLiveVisualsUpdate)
            {
                liveUpdateTime += Time.deltaTime;
                if (liveUpdateTime > 0.5f)
                {
                    ArrangeHand(animationSpeed / 10f);
                    liveUpdateTime = 0;
                }
            }
        }

        #region PUBLIC HAND API
        public void DiscardAll()
        {
            while (cards.Count > 0)
            {
                Discard(cards[0]);
            }
        }

        public void Discard(CardViewer card)
        {
            card.ClearEvents();
            RemoveCardFromHand(card);
            RelocateCard(card, 0, 0, animationSpeed);
            card.Discard(animationSpeed);
            Destroy(card.gameObject, 3);
        }
        
        public void AddCard(CardViewer card)
        {
            cards.Add(card);
            card.gameObject.transform.SetParent(hand);
            card.transform.localPosition = deck.transform.localPosition;
            card.transform.localScale = Vector3.one;

            ArrangeHand(animationSpeed);
            StartCoroutine(ListenToCardEvents(card));
        }
        
        public void AddEnergy(int amount)
        {
            currentEnergy += amount;
            currentEnergy = Math.Min(currentEnergy, energy);
        }

        public int GetEnergy()
        {
            return currentEnergy;
        }
        #endregion

        #region PRIVATE HAND HANDLING METHODS
        IEnumerator ListenToCardEvents(CardViewer card)
        {
            yield return new WaitForSeconds(animationSpeed);
            card.OnHover += Card_OnHover;
            card.OnStopHover += Card_OnStopHover;
            card.OnHold += Card_OnHold;
            card.OnRelease += Card_OnRelease;
        }

        private void ArrangeHand(float duration)
        {
            cardPositions = new Vector3[cards.Count];
            
            var xSpacing = cardXSpacing;
            var mid = cards.Count / 2;

            if (cards.Count % 2 == 1)
            {
                cardPositions[mid] = new Vector3(0, cardYSpacing, 0);

                RelocateCard(cards[mid], 0, cardYSpacing, duration);
                mid++;
                xSpacing = cardXSpacing;
            }

            for (var i = mid; i < cards.Count; i++)
            {
                cardPositions[i] = new Vector3(xSpacing, cardYSpacing, 0);
                cardPositions[cards.Count - i - 1] = new Vector3(-xSpacing, cardYSpacing, 0);
           
                RelocateCard(cards[i], xSpacing, cardYSpacing, duration);
                RelocateCard(cards[cards.Count - i - 1], -xSpacing, cardYSpacing, duration);

                xSpacing += cardXSpacing;
            }
        }
        private void RelocateCard(CardViewer card, float x, float y, float duration)
        {
            PositionCard(card, x, y, duration);
        }

        private void PositionCard(CardViewer card, float x, float y, float duration)
        {
            card.transform.TweenMove(new Vector3(x, cardY + y, 0), duration);
        }
        private void RotateCard(CardViewer card, float angle, float duration)
        {
            card.transform.TweenRotate(new Vector3(0, 0, angle), duration);
        }

        private void Card_OnRelease(CardViewer card)
        {
            if (currentCardHoldIdx < 0)
                return;
            Vector3 mousePosition = Input.mousePosition;

            if (RectTransformUtility.RectangleContainsScreenPoint(holderRect, mousePosition))
            {
                ReturnCardToHand(card);
            }
            else
            {
                UseCard(card);
            }
            
            currentCardHold = null;
            currentCardHoldIdx = -1;
            card.SetFloating(false);

            if (currentArrow != null)
            {
                Destroy(currentArrow.gameObject);
                currentArrow = null;
            }

        }
        private void UseCard(CardViewer card)
        {
            OnCardUsed?.Invoke(card.cardData);
            
            card.Dissolve(animationSpeed);
            Destroy(card.gameObject, 2);
        }

        private void ReturnCardToHand(CardViewer card)
        {
            if (currentCardHoldIdx < cards.Count)
            {
                cards.Insert(currentCardHoldIdx, card);
            }
            else
                cards.Add(card);
            
            card.gameObject.transform.SetParent(hand);
            card.gameObject.transform.SetSiblingIndex(currentCardHoldIdx);
            card.transform.rotation = Quaternion.identity;
            card.transform.localScale = Vector3.one;

            ArrangeHand(animationSpeed / 5f);
        }

        private void Card_OnHold(CardViewer card)
        {
            if (!cards.Contains(card))
                return;

            currentCardHold = card;
            currentCardHoldIdx = cards.IndexOf(card);
            card.SetVisualsPosition(Vector3.zero);
            card.transform.SetParent(cardHold);
            RemoveCardFromHand(card);

            if (card.cardData.IsTargetable)
            {
                card.SetFloating(true);
                currentArrow = Instantiate<TargetingArrow>(arrowPrefab, card.transform);
                RelocateCard(card, card.transform.localPosition.x,
                    card.transform.localPosition.y + 200, 0);
            }

        }

        private void RemoveCardFromHand(CardViewer card)
        {
            currentCardPreview = null;
            cards.Remove(card);
            ArrangeHand(animationSpeed / 2f);
        }

        private void Card_OnHover(CardViewer card)
        {
            if (currentCardPreview == card || currentCardHold != null || !cards.Contains(card))
                return;

            currentCardPreview = card;
            currentCardPreviewIdx = cards.IndexOf(currentCardPreview);
            card.SetVisualsPosition(Vector3.up * 10);
            card.SetFloating(true);
            
            if (currentCardPreviewIdx < cards.Count - 1)
            {
                Vector3 p1 = cardPositions[currentCardPreviewIdx + 1];
                PositionCard(cards[currentCardPreviewIdx + 1], p1.x + cardXSpacing / 3, p1.y, animationSpeed / 5f);
            }
            if (currentCardPreviewIdx > 0)
            {
                Vector3 p1 = cardPositions[currentCardPreviewIdx - 1];
                PositionCard(cards[currentCardPreviewIdx - 1], p1.x - cardXSpacing / 3, p1.y, animationSpeed / 5f);
            }

            card.transform.SetParent(cardPreview);
        }
        private void Card_OnStopHover(CardViewer card)
        {
            if (currentCardPreview != card)
                return;
            
            card.SetVisualsPosition(Vector3.zero);
            card.transform.SetParent(hand);
            card.transform.SetSiblingIndex(currentCardPreviewIdx);
            card.SetFloating(false);
            
            if (currentCardPreviewIdx < cards.Count - 1)
            {
                Vector3 p1 = cardPositions[currentCardPreviewIdx + 1];
                PositionCard(cards[currentCardPreviewIdx + 1], p1.x, p1.y, animationSpeed / 10);
            }
            if (currentCardPreviewIdx > 0)
            {
                Vector3 p1 = cardPositions[currentCardPreviewIdx - 1];
                PositionCard(cards[currentCardPreviewIdx - 1], p1.x, p1.y, animationSpeed / 10);
            }
            
            currentCardPreview = null;
            currentCardPreviewIdx = -1;
        }

        #endregion
    }
}