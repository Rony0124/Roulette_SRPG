using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public partial class CardsHolder
    {
        public struct CardPattern
        {
            public CardPatternType PatternType;
            public float Modifier;
        }

        private CardPattern currentPattern;
        public CardPattern CurrentPattern => currentPattern;
        
        private void CheckCardPatternOnHand()
        {
            if (currentPokerCardSelected is not { Count: > 0 })
                return;
            
            var rankGroups = currentPokerCardSelected
                .GroupBy(card => card.cardData.Number)
                .OrderByDescending(group => group.Count())
                .ToList();
            
            var suitGroups = currentPokerCardSelected
                .GroupBy(card => card.cardData.Type)
                .OrderByDescending(group => group.Count())
                .ToList();
            
            var sortedRanks = currentPokerCardSelected
                .Select(card => card.cardData.Number)
                .Distinct()
                .OrderBy(rank => rank)
                .ToList();
            
            string detectedPattern = null;

            if (CheckForStraightFlush(currentPokerCardSelected))
            {
                currentPattern.PatternType = CardPatternType.StraightFlush;
                currentPattern.Modifier = 180;
                detectedPattern = "Straight Flush";
            }
            else if (rankGroups.Any(g => g.Count() == 4))
            {
                currentPattern.PatternType = CardPatternType.FourOfKind;
                currentPattern.Modifier = 120;
                detectedPattern = "Four of a Kind";
            }
            else if (rankGroups.Any(g => g.Count() == 3) && rankGroups.Any(g => g.Count() == 2))
            {
                currentPattern.PatternType = CardPatternType.FullHouse;
                currentPattern.Modifier = 100;
                detectedPattern = "Full House";
            }
            else if (suitGroups.Any(g => g.Count() >= 5))
            {
                currentPattern.PatternType = CardPatternType.Flush;
                detectedPattern = "Flush";
                currentPattern.Modifier = 40;
            }
            else if (CheckForStraight(sortedRanks))
            {
                currentPattern.PatternType = CardPatternType.Straight;
                detectedPattern = "Straight";
                currentPattern.Modifier = 25;
            }
            else if (rankGroups.Any(g => g.Count() == 3))
            {
                currentPattern.PatternType = CardPatternType.ThreeOfKind;
                detectedPattern = "Three of a Kind";
                currentPattern.Modifier = 8;
            }
            else if (rankGroups.Count(g => g.Count() == 2) >= 2)
            {
                currentPattern.PatternType = CardPatternType.TwoPair;
                detectedPattern = "Two Pair";
                currentPattern.Modifier = 4;
            }
            else if (rankGroups.Any(g => g.Count() == 2))
            {
                currentPattern.PatternType = CardPatternType.OnePair;
                currentPattern.Modifier = 2;
                detectedPattern = "One Pair";
            }
            else
            {
                currentPattern.PatternType = CardPatternType.HighCard;
                currentPattern.Modifier = 1;
                detectedPattern = "High Card";
            }

            Debug.Log($"Highest Pattern Detected: {detectedPattern}");
        }
        
        private bool CheckForStraightFlush(List<PokerCard> cards)
        {
            var suitGroups = cards.GroupBy(card => card.cardData.Type);

            foreach (var suitGroup in suitGroups)
            {
                var sortedRanks = suitGroup
                    .Select(card => card.cardData.Number)
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
