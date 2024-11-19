using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Transform cardHold;
        
        private List<PokerCard> cardsOnHand;
        private Vector3[] cardPositions;
        
        private float liveUpdateTime;
        private int currentEnergy;
        private int currentHeart;
        private int currentCapacity;
        
        public int CurrentHeart => currentHeart;
        public int CurrentEnergy => currentEnergy;
        public int CurrentCapacity => currentCapacity;
        public List<PokerCard> CardsOnHand => cardsOnHand;
        
        public const int DefaultEnergy = 5;
        public const int DefaultHeart = 5;
        public const int DefaultCapacity = 8;
        
        private void Awake()
        {
            holderRect = GetComponent<RectTransform>();
            currentPokerCardSelected = new();
            cardsOnHand = new();
            
            InitializeDeck();
            
            currentEnergy = DefaultEnergy;
            currentHeart = DefaultHeart;
            currentCapacity = DefaultCapacity;
        }
        
        void Update()
        {
            if (currentPokerCardHold != null)
            {
                if (!currentPokerCardHold.cardData.IsTargetable)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    currentPokerCardHold.transform.position = mousePosition;
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
        
        public void DiscardSelectedCard()
        {
            foreach (var card in currentPokerCardSelected)
            {
                Discard(card);
            }
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
        
        public void AddEnergy(int amount)
        {
            currentEnergy += amount;
            currentEnergy = Math.Min(currentEnergy, DefaultEnergy);
        }
        
        public void UseCardsOnHand()
        {
            foreach (var selectedCard in currentPokerCardSelected)
            {
                OnCardUsed?.Invoke(selectedCard.cardData);
            
                selectedCard.Dissolve(animationSpeed);
                Destroy(selectedCard.gameObject, 2);    
            }
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
    }
}