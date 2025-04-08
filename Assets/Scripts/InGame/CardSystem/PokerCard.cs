using System;
using MoreMountains.Feedbacks;
using TCGStarter.Tweening;
using TMPro;
using TSoft.Data.Card;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.InGame.CardSystem
{
     public class PokerCard : MonoBehaviour
    {
        public event Action<PokerCard> OnHover;
        public event Action<PokerCard> OnStopHover;
        public event Action<PokerCard> OnClick;
        public event Action<PokerCard> OnHold;
        public event Action<PokerCard> OnRelease;

        [Header("Card Details")]
        [SerializeField] private SpriteRenderer imgBG;
        
        [Header("System Helpers")]
        [SerializeField] private GameObject Visuals;


        [HideInInspector] public CardInfo cardData;
        private bool isHeld = false;
        private bool isFloating = false;
        private Vector3 basePosition = Vector3.zero;

        [Header("UI")] 
        [SerializeField] private GameObject cardInfoUI;
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI cardDescription;
        
        [Header("Feedback")]
        public float duration = 0.7f;
        public MMFeedbacks onJokerUseFeedback;
        
        public bool IsFloating => isFloating;
        public bool IsHeld => isHeld;

        private void Update()
        {
            if (isFloating)
            {
                HandleFloating();
            }
        }

        public void SetData(CardInfo card)
        {
            cardData = card;
            imgBG.sprite = card.image;
        }

        public void Dissolve(float animationSpeed)
        {
         //   HitBox.SetActive(false);
            
            //imgBG.TweenFade(0f, animationSpeed, false);
            imgBG.transform.TweenScale(Vector3.one * 0.2f, animationSpeed, false);
            transform.TweenMoveY(300, 1f, false);
            Visuals.transform.TweenMoveY(300, 1f, false);
        }

        public void SetFloating(bool isEnable)
        {
            isFloating = isEnable;
            if (!isFloating)
            {
                Visuals.transform.localPosition = basePosition;
            }
        }

        private void HandleFloating()
        {
            Visuals.transform.localPosition = Vector3.Lerp(
                basePosition, basePosition + Vector3.up * 0.2f,
                Mathf.SmoothStep(0, 1, Mathf.PingPong(Time.time, 1)));
        }

        public void Discard(float animationSpeed)
        {
         //   HitBox.SetActive(false);
            transform.TweenScale(Vector3.zero, animationSpeed, false);
        }


        public void SetVisualsPosition(Vector3 newPos)
        {
            Visuals.transform.localPosition = newPos;
            basePosition = newPos;
        }

        public void OnMouseOver()
        {
            if (isHeld)
                return;
            
            OnHover?.Invoke(this);
            
            cardInfoUI.gameObject.SetActive(true);
            cardName.text = cardData.title;
            cardDescription.text = cardData.description;
        }

        public void OnMouseExit()
        {
            if (isHeld)
                return;
            
            OnStopHover?.Invoke(this);
            cardInfoUI.gameObject.SetActive(false);
        }

        public void OnMouseUpAsButton()
        {
            OnClick?.Invoke(this);
        }

        public void OnMouseDown()
        {
            OnHold?.Invoke(this);
            isHeld = true;
        }

        public void OnMouseUp()
        {
            OnRelease?.Invoke(this);
            isHeld = false;
        }

        internal void ClearEvents()
        {
            OnClick = null;
            OnHover = null;
            OnStopHover = null;
            OnHold = null;
            OnRelease = null;
        }

        private void OnDestroy()
        {
            //imgBG.TweenKill();
            gameObject.transform.TweenKillAll();
            Visuals.transform.TweenKillAll();
        }
        
        public void PositionCard(float x, float y, float duration)
        {
            transform.TweenMove(new Vector3(x, y, 0), duration);
        }
        public void RotateCard(float angle, float duration)
        {
            transform.TweenRotate(new Vector3(0, 0, angle), duration);
        }

        public void SetCardOrder(int order)
        {
            imgBG.sortingOrder = order;
        }
    }
}
