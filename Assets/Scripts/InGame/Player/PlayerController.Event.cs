using System.Collections;
using TSoft.InGame.CardSystem;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController
    {
        [Header("Visuals")]
        [SerializeField] private float cardY;
        [SerializeField] private float cardXSpacing;
        [SerializeField] private float cardYSpacing;
        [Range(0, 5)]
        [SerializeField] private float cardAngle = 3;
        
        [Range(0.2f, 2f)]
        [SerializeField] private float animationSpeed;
        
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
            pokerCard.SetCardDetails(true);
        }
        
        private void Card_OnStopHover(PokerCard pokerCard)
        {
            pokerCard.SetCardDetails(false);
        }

        private void OnClickJoker(PokerCard pokerCard)
        {
            if (pokerCard.cardData.policy == CustomEffectPolicy.Instant)
            {
                pokerCard.cardData.effect.ApplyEffect(this);
            }
            else
            {
                customEffects.Enqueue(pokerCard.cardData.effect);
            }
        }

        private void OnClickNormal(PokerCard pokerCard)
        {
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
