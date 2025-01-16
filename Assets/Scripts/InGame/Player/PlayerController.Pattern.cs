using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TSoft.Data.Skill;
using TSoft.InGame.CardSystem;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public partial class PlayerController
    {
        public Action<CardPattern> onPatternSelected;
        
        [Serializable]
        public class CardPattern
        {
            public CardPatternType PatternType;
            public float Modifier;
            public SkillSO skill;
        }
        
        //test 용으로 inspector에서 편집 가능하도록 설정
        [Header("Card Pattern")]
        [SerializeField][TableList]
        private List<CardPattern> defaultCardPatterns;
            
        private List<CardPattern> cardPatterns;

        private CardPattern currentPattern;

        public CardPattern CurrentPattern
        {
            get => currentPattern;
            set
            {
                if (value is not null)
                {
                    onPatternSelected?.Invoke(value);
                }
                
                currentPattern = value;
            }
        }

        public Dictionary<CardPatternType, ParticleSystem> particleDictionary;

        private void InitPattern()
        {
            if (particleDictionary is {Count: > 0})
            {
                foreach (var ps in particleDictionary.Values)
                {
                    Destroy(ps.gameObject);
                }
            }
            
            cardPatterns = new();
            particleDictionary = new();
            currentPattern = new();
            
            foreach (var defaultCardPattern in defaultCardPatterns)
            {
                var particleObj = Instantiate(defaultCardPattern.skill.skillParticleObj, transform);
                var particle = particleObj.GetComponent<ParticleSystem>();
                
                particleDictionary.Add(defaultCardPattern.PatternType, particle);
                cardPatterns.Add(defaultCardPattern);
            }
        }
        
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
            
            CardPatternType patternType;

            if (CheckForStraightFlush(currentPokerCardSelected))
            {
                patternType = CardPatternType.StraightFlush;
            }
            else if (rankGroups.Any(g => g.Count() == 4))
            {
                patternType = CardPatternType.FourOfKind;
            }
            else if (rankGroups.Any(g => g.Count() == 3) && rankGroups.Any(g => g.Count() == 2))
            {
                patternType = CardPatternType.FullHouse;
            }
            else if (suitGroups.Any(g => g.Count() >= 5))
            {
                patternType = CardPatternType.Flush;
            }
            else if (CheckForStraight(sortedRanks))
            {
                patternType = CardPatternType.Straight;
            }
            else if (rankGroups.Any(g => g.Count() == 3))
            {
                patternType = CardPatternType.ThreeOfKind;
            }
            else if (rankGroups.Count(g => g.Count() == 2) >= 2)
            {
                patternType = CardPatternType.TwoPair;
            }
            else if (rankGroups.Any(g => g.Count() == 2))
            {
                patternType = CardPatternType.OnePair;
            }
            else
            {
                patternType = CardPatternType.HighCard;
            }

            CurrentPattern = cardPatterns.Find(pattern => pattern.PatternType == patternType);
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
