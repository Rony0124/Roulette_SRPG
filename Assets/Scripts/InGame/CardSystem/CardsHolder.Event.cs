using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public partial class CardsHolder
    {
        [Header("Visuals")]
        [SerializeField] private float cardY;
        [SerializeField] private float cardXSpacing;
        [SerializeField] private float cardYSpacing;
        [Range(0.2f, 2f)]
        [SerializeField] private float animationSpeed;
        
        private PokerCard currentPokerCardPreview;
        private PokerCard currentPokerCardHold;

        private List<PokerCard> currentPokerCardSelected;
        
        private int currentCardPreviewIdx;

        private const int HandCountMax = 5;
        
        private RectTransform holderRect;
        
        IEnumerator ListenCardEvents(PokerCard pokerCard)
        {
            yield return new WaitForSeconds(animationSpeed);
            pokerCard.OnHover += Card_OnHover;
            pokerCard.OnStopHover += Card_OnStopHover;
            pokerCard.OnClick += Card_OnClick;
        }
        
        private void Card_OnClick(PokerCard pokerCard)
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
            
            Debug.Log($"selected card {pokerCard.cardData.Image.name}");
            

            CheckCardPatternOnHand();
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
    }
}
