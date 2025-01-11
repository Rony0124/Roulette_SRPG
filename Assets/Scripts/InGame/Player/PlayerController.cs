using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using TCGStarter.Tweening;
using TSoft.Data.Registry;
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
        [SerializeField] private Transform deck;
        
        private Gameplay gameplay;
        private AbilityContainer abilityContainer;
        
        private Vector3[] cardPositions;
        private Vector3[] cardRotations;
    
        //cards
        [SerializeField]
        private List<PokerCard> cardsOnHand;
        private List<PokerCard> currentPokerCardSelected;
        
        private Queue<CustomEffect> customEffects_joker;
        
        public bool CanMoveNextCycle { get; set; }
        
        public AbilityContainer AbilityContainer => abilityContainer;
        public Gameplay Gameplay =>  gameplay;

        public float currentDamage { get; set; }
        public float currentDamageModifier { get; set; }
        
        private const int HandCountMax = 5;
        
        protected override void InitOnDirectorChanged()
        {
            currentPokerCardSelected = new List<PokerCard>();
            cardsOnHand = new List<PokerCard>();
            customEffects_joker = new Queue<CustomEffect>();

            gameplay = GetComponent<Gameplay>();
            abilityContainer = GetComponent<AbilityContainer>();

            gameplay.Init();
            abilityContainer.Init();
            
            LoadSaveItems();
        }

        protected override async UniTask OnGameReady()
        {
            InitializeDeck();
            onGameReady?.Invoke();
            
            await UniTask.WaitForSeconds(1);
        }
        
        protected override async UniTask OnGameFinishSuccess()
        {
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
            currentDamage = gameplay.GetAttr(GameplayAttr.BasicAttackPower);
            
            //카드 패턴에 의한 데미지 추가
            currentDamageModifier *= CurrentPattern.Modifier;
            
            Debug.Log("damage" + currentDamage);
            
            //조커 이팩트 적용
            //단일 적용
            while (customEffects_joker.Count > 0)
            {
                var ce = customEffects_joker.Dequeue();
                ce.ApplyEffect(this);
            }
            
            //카드 삭제
            foreach (var selectedCard in currentPokerCardSelected)
            {
                selectedCard.Dissolve(animationSpeed);
                        
                Discard(selectedCard);
            }

            currentPokerCardSelected.Clear();
            
            var ultDmg = currentDamage * currentDamageModifier;
            
            var isDead = director.CurrentField.CurrentMonster.TakeDamage((int)ultDmg);
            
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
            var count = gameplay.Attributes.Count;
            Rect rc = new Rect(400, 300, 400, 20);
            GUI.Label(rc, $"Player Attribute");
            rc.y += 25;
        
            for (var i = 0; i < count; i++)
            {
                GUI.Label(rc, $"{gameplay.Attributes[i].attrType} : {gameplay.Attributes[i].value.CurrentValue}");
                rc.y += 25;
            }
        }
#endif
    }
}