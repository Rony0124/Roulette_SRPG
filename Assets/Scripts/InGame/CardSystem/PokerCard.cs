using System;
using TCGStarter.Tweening;
using TMPro;
using TSoft.Data.Card;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.InGame.CardSystem
{
    public class PokerCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<PokerCard> OnHover;
        public event Action<PokerCard> OnStopHover;
        public event Action<PokerCard> OnClick;
        public event Action<PokerCard> OnHold;
        public event Action<PokerCard> OnRelease;

        [Header("Card Details")]
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDescription;
        [SerializeField] private GameObject cardInfoObj;
        [SerializeField] private Image imgBG;
        
        [Header("System Helpers")]
        [SerializeField] private GameObject Visuals;
        [SerializeField] private GameObject HitBox;

        [HideInInspector] public CardSO cardData;
        private bool isHeld = false;
        private bool isFloating = false;
        private Vector3 basePosition = Vector3.zero;
        
        public bool IsFloating => isFloating;
        public bool IsHeld => isHeld;

        private void Update()
        {
            if (isFloating)
            {
                HandleFloating();
            }
        }

        public void SetData(CardSO card)
        {
            cardData = card;
            txtTitle.text = card.title;
            txtDescription.text = card.description;
            imgBG.sprite = card.image;
        }

        public void Dissolve(float animationSpeed)
        {
            HitBox.SetActive(false);

            txtTitle.TweenFade(0f, animationSpeed / 4, false);
            txtDescription.TweenFade(0f, animationSpeed / 4, false);
            imgBG.TweenFade(0f, animationSpeed, false);
            imgBG.transform.TweenScale(Vector3.one * 1.2f, animationSpeed, false);

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

        public void SetCardDetails(bool isEnable)
        {
            cardInfoObj.SetActive(isEnable);
        }

        private void HandleFloating()
        {
            Visuals.transform.localPosition = Vector3.Lerp(
                basePosition, basePosition + Vector3.up * 7,
                Mathf.SmoothStep(0, 1, Mathf.PingPong(Time.time, 1)));
        }

        public void Discard(float animationSpeed)
        {
            HitBox.SetActive(false);
            transform.TweenScale(Vector3.zero, animationSpeed, false);
        }


        public void SetVisualsPosition(Vector3 newPos)
        {
            Visuals.transform.localPosition = newPos;
            basePosition = newPos;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isHeld)
                return;
            
            OnHover?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isHeld)
                return;
            
            OnStopHover?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnHold?.Invoke(this);
            isHeld = true;
        }

        public void OnPointerUp(PointerEventData eventData)
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
            imgBG.TweenKill();
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
    }
}
