using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
using Unity.VisualScripting;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController
    {
        public Func<PokerCard, bool> onClickCard;
        public Action<PokerCard> onJokerUse;
        
        [Header("Visuals")]
        [SerializeField] private float cardY;
        [SerializeField] private float cardXSpacing;
        [SerializeField] private float cardYSpacing;
        [Range(0, 5)]
        [SerializeField] private float cardAngle = 3;
        
        [Range(0.2f, 2f)]
        [SerializeField] private float animationSpeed;
        
        [Header("Info")]
        [SerializeField] private float infoHeight;
        [SerializeField] private GameObject infoObject;
        [SerializeField] private TextMeshProUGUI infoTitle;
        [SerializeField] private TextMeshProUGUI infoDescription;

        [HideInInspector]
        public bool isSelectingCardOnHand;

        private int jokerUsedNumber = 0;
        public int JokerUsedNumber => jokerUsedNumber;
        
        IEnumerator ListenCardEvents(PokerCard pokerCard)
        {
            yield return new WaitForSeconds(animationSpeed);
            pokerCard.OnHover += Card_OnHover;
            pokerCard.OnStopHover += Card_OnStopHover;
            pokerCard.OnClick += Card_OnClick;
        }
        
        private void Card_OnClick(PokerCard pokerCard)
        {
            if (pokerCard.cardData.type == CardType.Joker)
            {
                OnClickJoker(pokerCard);
            }
            else
            {
                OnClickNormal(pokerCard);    
            }
        }
        
        private void Card_OnHover(PokerCard pokerCard)
        {
            var pos = pokerCard.transform.position;
            pos.y = infoHeight;
            
            infoObject.transform.position = pos;
            
            infoObject.SetActive(true);
            infoTitle.text = pokerCard.cardData.title;
            infoDescription.text = pokerCard.cardData.description;
         //   pokerCard.SetCardDetails(true);
        }
        
        private void Card_OnStopHover(PokerCard pokerCard)
        {
            infoObject.SetActive(false);
        }

        private async UniTaskVoid OnClickJoker(PokerCard pokerCard)
        {
            jokerUsedNumber++;
            
            pokerCard.onJokerUseFeedback?.PlayFeedbacks();
            onJokerUse?.Invoke(pokerCard);

            await UniTask.WaitForSeconds(pokerCard.duration);
            
            pokerCard.cardData.instantEffect?.effect?.ApplyEffect(director).Forget();
            
            if (pokerCard.cardData.effect)
            {
                gameplay.AddEffect(pokerCard.cardData.effect);
            }
        }

        private void OnClickNormal(PokerCard pokerCard)
        {
            if (isSelectingCardOnHand)
            {
                if (onClickCard(pokerCard))
                {
                    isSelectingCardOnHand = false;
                }
                
                return;
            }
            
            if (pokerCard.IsFloating)
            {
                pokerCard.SetVisualsPosition(Vector3.zero);
                pokerCard.SetFloating(false);
                
                var cardIdx = cardsOnHand.IndexOf(pokerCard);
                RotateCard(pokerCard, cardRotations[cardIdx].z, animationSpeed / 10);
                
                currentPokerCardSelected.Remove(pokerCard);
            }
            else
            {
                if(currentPokerCardSelected.Count >= HandCountMax)
                    return;
                
                pokerCard.SetVisualsPosition(Vector3.up * 100);
                RotateCard(pokerCard, 0, 0);
                pokerCard.SetFloating(true);
                
                currentPokerCardSelected.Add(pokerCard);
            }
            
            CheckCardPatternOnHand();
        }
    }
}
