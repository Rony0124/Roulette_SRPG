using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using TCGStarter.Tweening;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController : MonoBehaviour
    {
        [Header("Positions")]
        [SerializeField] private Transform hand;
        [SerializeField] private Transform deck;
        
        private Vector3[] cardPositions;
        private Vector3[] cardRotations;
    
        //cards
        [SerializeField]
        private List<PokerCard> cardsOnHand;
        private List<PokerCard> currentPokerCardSelected;
        public List<PokerCard> CurrentPokerCardSelected => currentPokerCardSelected;
        
        private Gameplay gameplay;
        public Gameplay Gameplay =>  gameplay;

        private bool isSubmitting;
        
        private const int HandCountMax = 5;
        
        private void Awake()
        {
            currentPokerCardSelected = new List<PokerCard>();
            cardsOnHand = new List<PokerCard>();

            gameplay = GetComponent<Gameplay>();
            gameplay.Init();
            
            InitializeDeck();
            InitializePattern();
            InitializeEquipment();
        }

        public async UniTask GameStart()
        {
            await gameplay.OnRoundBegin();
            
            Debug.Log("player game Start");
            
            DrawCards();
        }
        
        private async UniTask OnPostPlaySuccess()
        {
            await gameplay.OnRoundFinished();
            
            await UniTask.WaitForSeconds(2);
            
            DiscardAll();
        }

        public bool CanPlay()
        {
            if (CombatController.Instance.phase != GamePhase.Main)
                return false;

            if (CombatController.Instance.currentTurn != 0)
                return false;
            
            if (isSubmitting)
                return false;
            
            return true;
        }

        public async UniTask<bool> TryUseCardsOnHand()
        {
            if (!CanPlay())
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
            await currentPattern.skill.PlaySkill(this, CombatController.Instance.Monster);

            //turn finished 이팩트 추가
            await gameplay.OnTurnFinished();

            gameplay.ResetAttributeModifiers();

            isSubmitting = false;

            jokerUsedNumber = 0;

            CombatController.Instance.EndTurn().Forget();

            return true;
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
        
#if UNITY_EDITOR
        /*void OnGUI()
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
        }*/
#endif
    }
}