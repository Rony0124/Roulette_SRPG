using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using TCGStarter.Tweening;
using TSoft.Data.Registry;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController : ControllerBase
    {
        public Action onGameReady;
   
        [Header("Positions")]
        [SerializeField] private Transform hand;
        [SerializeField] private Transform deck;
        
        private Gameplay gameplay;
        private AbilityContainer abilityContainer;
        
        private Vector3[] cardPositions;
        private Vector3[] cardRotations;
    
        //cards
        [SerializeField]
        private List<PokerCard> cardsOnHand;
        private List<PokerCard> currentPokerCardSelected;
        
        public List<PokerCard> CurrentPokerCardSelected => currentPokerCardSelected;
        
        public bool CanMoveNextCycle { get; set; }
        
        public Gameplay Gameplay =>  gameplay;

        private bool isSubmitting;
        
        private const int HandCountMax = 5;
        
        protected override void InitOnDirectorChanged()
        {
            currentPokerCardSelected = new List<PokerCard>();
            cardsOnHand = new List<PokerCard>();

            gameplay = GetComponent<Gameplay>();
            abilityContainer = GetComponent<AbilityContainer>();

            gameplay.Init();
            abilityContainer.Init();
            
            LoadSaveItems();
        }

        protected override async UniTask OnPrePlay()
        {
            InitializeDeck();
            InitPattern();
            onGameReady?.Invoke();

            await gameplay.OnRoundBegin();
            
            await UniTask.WaitForSeconds(1);
        }
        
        protected override async UniTask OnPostPlaySuccess()
        {
            await gameplay.OnRoundFinished();
            
            await UniTask.WaitForSeconds(2);
            await UniTask.WaitWhile(() => !CanMoveNextCycle);
            
            DiscardAll();
        }

        private void LoadSaveItems()
        {
            var artifactRegistryIds = DataRegistry.Instance.ArtifactRegistry.Ids;
            var jokerRegistryIds = DataRegistry.Instance.JokerRegistry.Ids;
            
            foreach (var id in artifactRegistryIds)
            {
                if (!GameSave.Instance.HasItemsId(id.Guid))
                {
                    continue;
                }

                var artifact = DataRegistry.Instance.ArtifactRegistry.Get(id);
                abilityContainer.currentArtifacts.Add(artifact);
            }

            foreach (var id in jokerRegistryIds)
            {
                if (!GameSave.Instance.HasItemsId(id.Guid))
                {
                    continue;
                }
                
                var joker = DataRegistry.Instance.JokerRegistry.Get(id);
                specialCardDB.Add(joker);
            }
        }
        
        public async UniTask<bool> TryUseCardsOnHand()
        {
            if (isSubmitting)
                return false;
            
            var currentHeart = gameplay.GetAttr(GameplayAttr.Heart, false);
            if (currentHeart <= 0)
                return false;
            
            //손에 들고 있는 카드가 없다면 false
            if (currentPokerCardSelected.IsNullOrEmpty())
                return false;
            
            isSubmitting = true;
            
            //하트 사용
            --currentHeart;
            gameplay.SetAttr(GameplayAttr.Heart, currentHeart, false);
            
            gameplay.CaptureCurrentAttributeModifiers();
            
            //현재 패턴에 해당하는 이팩트 추가
            currentPattern.ApplyCurrentPattern(this);
            
            //turn begin 이팩트 추가
            await gameplay.OnTurnBegin();
            
            //카드 삭제
            DiscardSelectedCards();
            
            //스킬 플레이
            await currentPattern.skill.PlaySkill(this, director.CurrentMonster);
            
            //turn finished 이팩트 추가
            await gameplay.OnTurnFinished();
            
            gameplay.ResetAttributeModifiers();
            
            isSubmitting = false;
            
            jokerUsedNumber = 0;

            if (CheckGameOver())
                return false;
            
            return true;
        }

        private bool CheckGameOver()
        {
            var currentHeart = gameplay.GetAttr(GameplayAttr.Heart);
            if (director.CurrentMonster.IsDead)
            {
                if (currentHeart > 0)
                {
                    director.GameOver(true);
                    return true;
                }
            }
            else
            {
                if (currentHeart <= 0 || cardsOnHand.Count <= 0)
                {
                    director.GameOver(false);
                    return true;
                }
            }

            return false;
        }
        
        public bool TryDiscardSelectedCard()
        {
            var currentEnergy = gameplay.GetAttr(GameplayAttr.Energy);
            if(currentEnergy <= 0)
                return false;
            
            //손에 들고 있는 카드가 없다면 false
            if (currentPokerCardSelected.IsNullOrEmpty())
                return false;
            
            --currentEnergy;
            gameplay.SetAttr(GameplayAttr.Energy, currentEnergy);

            DiscardSelectedCards();
            
            return true;
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
        
#if UNITY_EDITOR
        void OnGUI()
        {
            var count = gameplay.attributes.Count;
            Rect rc = new Rect(400, 300, 400, 20);
            GUI.Label(rc, $"Player Attribute");
            rc.y += 25;
        
            for (var i = 0; i < count; i++)
            {
                GUI.Label(rc, $"{gameplay.attributes[i].attrType} : {gameplay.attributes[i].value.CurrentValue.Value}");
                rc.y += 25;
            }
        }
#endif
    }
}