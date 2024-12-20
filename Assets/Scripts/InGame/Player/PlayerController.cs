using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using TSoft.InGame.CardSystem;
using TSoft.InGame.CardSystem.CE;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController : ControllerBase
    {
        public Action onGameReady;
   
        [Header("Positions")]
        [SerializeField] private Transform hand;
        [SerializeField] private Transform cardPreview;
        [SerializeField] private Transform deck;
        
        private Gameplay gameplay;
        
        //animation
        private Vector3[] cardPositions;
        private int currentCardPreviewIdx;
        
        //cards
        [SerializeField]
        private List<PokerCard> cardsOnHand;
        private List<PokerCard> currentPokerCardSelected;
        
        private PokerCard currentPokerCardPreview;
        private PokerCard currentPokerCardHold;
        private Queue<CustomEffect> customEffects;
        
        public bool CanMoveNextCycle { get; set; }
       
        public List<PokerCard> CardsOnHand => cardsOnHand;
        
        public Gameplay Gameplay =>  gameplay;
        
        private const int HandCountMax = 5;
        
        protected override void InitOnDirectorChanged()
        {
            currentPokerCardSelected = new List<PokerCard>();
            cardsOnHand = new List<PokerCard>();
            customEffects = new Queue<CustomEffect>();

            gameplay = GetComponent<Gameplay>();
        }

        protected override async UniTask OnGameReady()
        {
            InitializeDeck();
            gameplay.Init();
            
            onGameReady?.Invoke();
            
            await UniTask.WaitForSeconds(1);
        }
        
        protected override async UniTask OnGameFinishSuccess()
        {
            await UniTask.WaitForSeconds(2);
            await UniTask.WaitWhile(() => !CanMoveNextCycle);
            DiscardAll();
        }
        
        public bool TryUseCardsOnHand()
        {
            var currentHeart = gameplay.GetAttr(GameplayAttr.Heart);
            if (currentHeart <= 0)
                return false;

            //손에 들고 있는 카드가 없다면 false
            if (currentPokerCardSelected.IsNullOrEmpty())
                return false;
            
            //하트는 먼저 깎아준다.
            --currentHeart;
            gameplay.SetAttr(GameplayAttr.Heart, currentHeart);
            
            //기본 데미지 적용
            var damage = gameplay.GetAttr(GameplayAttr.BasicAttackPower);
            
            //카드 패턴에 의한 데미지 추가
            damage *= CurrentPattern.Modifier;
            
            //카드 
            while (customEffects.Count > 0)
            {
                var ce = customEffects.Dequeue();
                ce.ApplyEffect(this);
            }
            
            foreach (var selectedCard in currentPokerCardSelected)
            {
                selectedCard.Dissolve(animationSpeed);
                        
                Discard(selectedCard);
            }

            currentPokerCardSelected.Clear();
            
            var isDead = director.CurrentField.TakeDamage((int)damage);
            
            if (isDead)
            {
                if (currentHeart > 0)
                {
                    director.GameOver(true);
                    return false;
                }
            }
            else
            {
                if (currentHeart <= 0)
                {
                    director.GameOver(false);
                    return false;
                }
            }
            
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
            
            currentPokerCardSelected.Clear();
            
            --currentEnergy;
            gameplay.SetAttr(GameplayAttr.Energy, currentEnergy);
            
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
            StartCoroutine((IEnumerator)ListenCardEvents(pokerCard));
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