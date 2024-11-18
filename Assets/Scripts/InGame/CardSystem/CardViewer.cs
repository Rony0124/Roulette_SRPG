using System;
using InGame.CardSystem;
using TCGStarter.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.InGame.CardSystem
{
    // Responsible for viewing the cards on the canvas
    // Fired user interactions events
    public class CardViewer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<CardViewer> OnHover;
        public event Action<CardViewer> OnStopHover;
        public event Action<CardViewer> OnClick;
        public event Action<CardViewer> OnHold;
        public event Action<CardViewer> OnRelease;

        [Header("Card Details")]
        [SerializeField] private TextMeshProUGUI txtTitle;
        [SerializeField] private TextMeshProUGUI txtDescription;
        [SerializeField] private TextMeshProUGUI txtCost;
        [SerializeField] private Image imgBG;

        [Header("Effects")]
        [SerializeField] private Image glow;
        [SerializeField] private Image extraGlow;
        [SerializeField] private Image line;

        [Header("System Helpers")]
        [SerializeField] private GameObject Visuals;
        [SerializeField] private GameObject HitBox;

        [HideInInspector] public CardData cardData;
        private bool isHeld = false;
        private bool isFloating = false;
        private Vector3 basePosition = Vector3.zero;

        private void Awake()
        {
            extraGlow.gameObject.SetActive(false);
        }
        private void Start()
        {
        }

        private void Update()
        {
            if (isFloating)
            {
                handleFloating();
            }

        }

        public void SetData(CardData card)
        {
            this.cardData = card;
            txtTitle.text = card.Title;
            txtDescription.text = card.Description;
            txtCost.text = card.Cost.ToString();
            imgBG.sprite = card.Image;
        }

        public void SetBasicGlow(bool isEnable)
        {
            if (isEnable)
            {
                glow.color = new Color(1, 1, 1, 1f);
                glow.TweenFade(0.3f, 1f, true);
            }
            else
            {
                glow.TweenKill();
                glow.color = new Color(1, 1, 1, 0);
            }
        }
        public void SetGlowExtra(bool isEnable)
        {
            if (isEnable)
            {
                extraGlow.gameObject.SetActive(true);
                line.gameObject.SetActive(true);

                extraGlow.transform.localScale = Vector3.one;
                extraGlow.transform.TweenScale(new Vector3(0.97f, 1, 1), 1.4f, true);
                extraGlow.transform.TweenScale(new Vector3(1, 0.97f, 1), 1f, true);
                line.transform.TweenMoveY(8, 1.2f, true);
            }
            else
            {
                extraGlow.transform.TweenKillAll();
                extraGlow.gameObject.SetActive(false);
                line.transform.TweenKillAll();
                line.gameObject.SetActive(false);
            }
        }

        public void Dissolve(float animationSpeed)
        {
            HitBox.SetActive(false);
            SetBasicGlow(false);

            txtTitle.TweenFade(0f, animationSpeed / 4, false);
            txtDescription.TweenFade(0f, animationSpeed / 4, false);
            txtCost.TweenFade(0f, animationSpeed / 4, false);
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

        private void handleFloating()
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

        public void HideVisuals()
        {
            Visuals.SetActive(false);
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
            glow.TweenKill();
            extraGlow.TweenKill();
            gameObject.transform.TweenKillAll();
            line.transform.TweenKillAll();
            Visuals.transform.TweenKillAll();
        }
    }
}
