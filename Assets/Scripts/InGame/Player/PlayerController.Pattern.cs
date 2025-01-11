using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TSoft.InGame.CardSystem;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController
    {
        [Serializable]
        public class CardPattern
        {
            public CardPatternType PatternType;
            public float Modifier;
        }
        
        //test 용으로 inspector에서 편집 가능하도록 설정
        [Header("Card Pattern")]
        [TableList]
        private List<CardPattern> defaultCardPatterns;

        private CardPattern currentPattern;
        public CardPattern CurrentPattern => currentPattern;
        
        private void CheckCardPatternOnHand()
        {
            if (currentPokerCardSelected is not { Count: > 0 })
                return;
            
            var rankGroups = currentPokerCardSelected
                .GroupBy(card => card.cardData.number)
                .OrderByDescending(group => group.Count())
                .ToList();
            
            var suitGroups = currentPokerCardSelected
                .GroupBy(card => card.cardData.type)
                .OrderByDescending(group => group.Count())
                .ToList();
            
            var sortedRanks = currentPokerCardSelected
                .Select(card => card.cardData.number)
                .Distinct()
                .OrderBy(rank => rank)
                .ToList();
            
            string detectedPattern = null;

            if (CheckForStraightFlush(currentPokerCardSelected))
            {
                currentPattern.PatternType = CardPatternType.StraightFlush;
                currentPattern.Modifier = 180;
            }
            else if (rankGroups.Any(g => g.Count() == 4))
            {
                currentPattern.PatternType = CardPatternType.FourOfKind;
                currentPattern.Modifier = 120;
            }
            else if (rankGroups.Any(g => g.Count() == 3) && rankGroups.Any(g => g.Count() == 2))
            {
                currentPattern.PatternType = CardPatternType.FullHouse;
                currentPattern.Modifier = 100;
            }
            else if (suitGroups.Any(g => g.Count() >= 5))
            {
                currentPattern.PatternType = CardPatternType.Flush;
                currentPattern.Modifier = 40;
            }
            else if (CheckForStraight(sortedRanks))
            {
                currentPattern.PatternType = CardPatternType.Straight;
                currentPattern.Modifier = 25;
            }
            else if (rankGroups.Any(g => g.Count() == 3))
            {
                currentPattern.PatternType = CardPatternType.ThreeOfKind;
                currentPattern.Modifier = 8;
            }
            else if (rankGroups.Count(g => g.Count() == 2) >= 2)
            {
                currentPattern.PatternType = CardPatternType.TwoPair;
                currentPattern.Modifier = 4;
            }
            else if (rankGroups.Any(g => g.Count() == 2))
            {
                currentPattern.PatternType = CardPatternType.OnePair;
                currentPattern.Modifier = 2;
            }
            else
            {
                currentPattern.PatternType = CardPatternType.HighCard;
                currentPattern.Modifier = 1;
            }

            Debug.Log($"Highest Pattern Detected: {detectedPattern}");
        }
        
        private bool CheckForStraightFlush(List<PokerCard> cards)
        {
            var suitGroups = cards.GroupBy(card => card.cardData.type);

            foreach (var suitGroup in suitGroups)
            {
                var sortedRanks = suitGroup
                    .Select(card => card.cardData.number)
                    .Distinct()
                    .OrderBy(rank => rank)
                    .ToList();

                if (CheckForStraight(sortedRanks))
                    return true;
            }

            return false;
        }
        
        private bool CheckForStraight(List<int> sortedRanks)
        {
            int consecutiveCount = 1;
            for (int i = 1; i < sortedRanks.Count; i++)
            {
                if (sortedRanks[i] == sortedRanks[i - 1] + 1)
                {
                    consecutiveCount++;
                    if (consecutiveCount >= 5)
                        return true;
                }
                else
                {
                    consecutiveCount = 1;
                }
            }
            return false;
        }
        
    }
}
