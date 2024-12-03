using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TSoft.InGame.GamePlaySystem;
using TSoft.Managers;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public partial class PlayerController : MonoBehaviour
    {
        [Header("Positions")]
        [SerializeField] private Transform hand;
        [SerializeField] private Transform cardPreview;
        [SerializeField] private Transform deck;
        
        private Gameplay gameplay;
        private InGameDirector director;
        
        //animation
        private Vector3[] cardPositions;
        private int currentCardPreviewIdx;
        
        //cards
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
            director = FindObjectOfType<DirectorBase>() as InGameDirector;
            
            InitializeDeck();
        }
        
        public bool TryUseCardsOnHand()
        {
            var currentHeart = gameplay.GetAttr(GameplayAttr.Heart);
            if (currentHeart <= 0)
                return false;

            if (currentPokerCardSelected.IsNullOrEmpty())
                return false;
            
            --currentHeart;
            
            gameplay.SetAttr(GameplayAttr.Heart, currentHeart);

            //현재 데미지 상태
            var damage = gameplay.GetAttr(GameplayAttr.BasicAttackPower);
            Debug.Log(damage);
            
            foreach (var selectedCard in currentPokerCardSelected)
            {
                selectedCard.Dissolve(animationSpeed);
                
                Discard(selectedCard);
            }
            
            //카드 패턴에 의한 데미지 추가
            damage *= CurrentPattern.Modifier;

            director.CurrentMonster.TakeDamage(damage);

            if (currentHeart <= 0 && director.CurrentMonster.Data.Hp > 0)
            {
                UIManager.Instance.ShowPopupUI(UIManager.PopupType.GameOver);
            }
            
            currentPokerCardSelected.Clear();
            
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
    }
}