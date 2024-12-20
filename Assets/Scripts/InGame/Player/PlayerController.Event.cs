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
            if (currentPokerCardPreview == pokerCard || currentPokerCardHold != null || !cardsOnHand.Contains(pokerCard))
                return;

            currentPokerCardPreview = pokerCard;
            currentCardPreviewIdx = cardsOnHand.IndexOf(currentPokerCardPreview);
            pokerCard.SetCardDetails(true);

            pokerCard.transform.SetParent(cardPreview);
        }
        
        private void Card_OnStopHover(PokerCard pokerCard)
        {
            if (currentPokerCardPreview != pokerCard)
                return;
            
            pokerCard.transform.SetParent(hand);
            pokerCard.transform.SetSiblingIndex(currentCardPreviewIdx);
            pokerCard.SetCardDetails(false);
            
            currentPokerCardPreview = null;
            currentCardPreviewIdx = -1;
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
                
                currentPokerCardSelected.Remove(pokerCard);
            }
            else
            {
                if(currentPokerCardSelected.Count >= HandCountMax)
                    return;
                
                pokerCard.SetVisualsPosition(Vector3.up * 10);
                pokerCard.SetFloating(true);
                
                currentPokerCardSelected.Add(pokerCard);
            }
            
            CheckCardPatternOnHand();
        }
    }
}
