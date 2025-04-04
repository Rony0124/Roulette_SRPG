using System;
using System.Collections.Generic;
using System.Linq;
using InGame;
using UnityEngine;

namespace HF.GamePlay
{
    public class CardPattern
    {
        public CardPatternType patternType;
        public List<Card> cards = new();
    }
    
    [Serializable]
    public class Player
    {
        public int player_id;
        public Guid cardback;
        
        public bool is_ai;
        public bool ready = false;
        
        public int hp;
        public int hp_max;
        public int mana = 0;
        public int mana_max = 0;
        
        public Dictionary<Guid, Card> cards_all = new Dictionary<Guid, Card>();
        
        public List<CardPattern> cards_pattern = new ();
        public List<Card> cards_deck = new List<Card>();
        public List<Card> cards_hand = new List<Card>();  
        public List<Card> cards_discard = new List<Card>();

        public CardPattern currentPattern;

        public Player(int id)
        {
            player_id = id;
            hp = 100;
            hp_max = 100;
        }

        public virtual int GetAttack()
        {
            //TODO 데미지 pattern으로 계산
            return 10;
        }
        
        public Card GetHandCard(Guid uid)
        {
            foreach (Card card in cards_hand)
            {
                if (card.uid == uid)
                    return card;
            }
            return null;
        }

        public void RemovePattern(CardPattern pattern)
        {
            foreach (var card in pattern.cards)
            {
                RemoveCardFromAllGroups(card);
            }
            
            cards_pattern.Clear();
        }
        
        public virtual void RemoveCardFromAllGroups(Card card)
        {
            cards_deck.Remove(card);
            cards_hand.Remove(card);
            cards_discard.Remove(card);
        }
        
        public virtual bool IsDead()
        {
            if (cards_hand.Count == 0 && cards_deck.Count == 0)
                return true;
            
            if (hp <= 0)
                return true;
            
            return false;
        }
        
        //Clone all player variables into another var, used mostly by the AI when building a prediction tree
        public static void Clone(Player source, Player dest)
        {
            dest.player_id = source.player_id;
            dest.is_ai = source.is_ai;

            dest.hp = source.hp;
            dest.hp_max = source.hp_max;
            dest.mana = source.mana;
            dest.mana_max = source.mana_max;
           
            dest.cards_pattern = source.cards_pattern;
            dest.currentPattern = source.currentPattern;
            
            Card.CloneDict(source.cards_all, dest.cards_all);
            Card.CloneListRef(dest.cards_all, source.cards_hand, dest.cards_hand);
            Card.CloneListRef(dest.cards_all, source.cards_deck, dest.cards_deck);
            Card.CloneListRef(dest.cards_all, source.cards_discard, dest.cards_discard);
        }

        public void CalculateCardPatternsOnHand()
        {
            cards_pattern.Clear();
            
            if (cards_hand == null || cards_hand.Count == 0)
                return;

            var grade = new bool[10];
            var gradeNumberCombined = new int[10];
            var patternCardsDict = new Dictionary<CardPatternType, List<Card>>();
            
            grade[(int)CardPatternType.None] = true;
            grade[(int)CardPatternType.HighCard] = true;

            int[] suits = new int[5];
            int[] numbers = new int[15];
            int[] suitsNumberCombined = new int[5];

            foreach (Card card in cards_hand)
            {
                numbers[card.Data.number]++;
                if (card.Data.number == 1)
                    numbers[14]++;

                int typeIdx = (int)card.Data.type;
                suits[typeIdx]++;
                suitsNumberCombined[typeIdx] += card.Data.number;
            }

            int biggestNumber = 0;
            for (int i = 14; i >= 0; i--)
            {
                if (numbers[i] > 0)
                {
                    biggestNumber = i;
                    break;
                }
            }

            gradeNumberCombined[(int)CardPatternType.HighCard] = biggestNumber;
            patternCardsDict[CardPatternType.HighCard] = new List<Card> { cards_hand.OrderByDescending(c => c.Data.number).First() };

            // STRAIGHT
            bool isStraight = false;
            for (int i = 1; i <= 10; i++)
            {
                List<Card> straightCards = new();
                bool valid = true;

                for (int j = i; j < i + 5; j++)
                {
                    if (numbers[j] == 0)
                    {
                        valid = false;
                        break;
                    }
                    straightCards.Add(cards_hand.First(c => c.Data.number == j || (j == 14 && c.Data.number == 1)));
                }

                if (valid)
                {
                    isStraight = true;
                    grade[(int)CardPatternType.Straight] = true;
                    gradeNumberCombined[(int)CardPatternType.Straight] = straightCards.Sum(c => c.Data.number);
                    patternCardsDict[CardPatternType.Straight] = straightCards;
                    break;
                }
            }

            // FLUSH
            bool isFlush = false;
            for (int i = 1; i < 5; i++)
            {
                if (suits[i] >= 5)
                {
                    isFlush = true;
                    grade[(int)CardPatternType.Flush] = true;
                    var flushCards = cards_hand.Where(c => (int)c.Data.type == i)
                                                .OrderByDescending(c => c.Data.number)
                                                .Take(5)
                                                .ToList();
                    gradeNumberCombined[(int)CardPatternType.Flush] = flushCards.Sum(c => c.Data.number);
                    patternCardsDict[CardPatternType.Flush] = flushCards;
                }
            }

            // STRAIGHT FLUSH
            if (isFlush && isStraight)
            {
                var sfCards = patternCardsDict[CardPatternType.Straight]
                    .Where(c => patternCardsDict[CardPatternType.Flush].Contains(c)).ToList();

                if (sfCards.Count == 5)
                {
                    grade[(int)CardPatternType.StraightFlush] = true;
                    gradeNumberCombined[(int)CardPatternType.StraightFlush] = sfCards.Sum(c => c.Data.number);
                    patternCardsDict[CardPatternType.StraightFlush] = sfCards;
                }
            }

            int pairCnt = 0;
            int tripleCnt = 0;
            int[] pairNumbers = new int[3];
            int tripleNumber = 0;
            List<Card> pairs = new();
            List<Card> triple = new();

            for (int i = 1; i <= 13; i++)
            {
                if (numbers[i] == 4)
                {
                    grade[(int)CardPatternType.FourOfKind] = true;
                    var four = cards_hand.Where(c => c.Data.number == i).ToList();
                    gradeNumberCombined[(int)CardPatternType.FourOfKind] = i * 4;
                    patternCardsDict[CardPatternType.FourOfKind] = four;
                }

                if (numbers[i] == 3)
                {
                    tripleCnt++;
                    tripleNumber = i;
                    triple = cards_hand.Where(c => c.Data.number == i).ToList();
                }

                if (numbers[i] == 2)
                {
                    pairCnt++;
                    pairNumbers[pairCnt] = i * 2;
                    pairs.AddRange(cards_hand.Where(c => c.Data.number == i));
                }
            }

            if (pairCnt == 1)
            {
                grade[(int)CardPatternType.OnePair] = true;
                gradeNumberCombined[(int)CardPatternType.OnePair] = pairNumbers[1];
                patternCardsDict[CardPatternType.OnePair] = pairs;
            }

            if (pairCnt == 2)
            {
                grade[(int)CardPatternType.TwoPair] = true;
                gradeNumberCombined[(int)CardPatternType.TwoPair] = pairNumbers[1] + pairNumbers[2];
                patternCardsDict[CardPatternType.TwoPair] = pairs;
            }

            if (tripleCnt == 1)
            {
                grade[(int)CardPatternType.ThreeOfKind] = true;
                gradeNumberCombined[(int)CardPatternType.ThreeOfKind] = tripleNumber;
                patternCardsDict[CardPatternType.ThreeOfKind] = triple;
            }

            if (tripleCnt == 1 && pairCnt >= 1)
            {
                grade[(int)CardPatternType.FullHouse] = true;
                gradeNumberCombined[(int)CardPatternType.FullHouse] = tripleNumber + pairNumbers[1];
                var fhCards = new List<Card>();
                fhCards.AddRange(triple);
                fhCards.AddRange(pairs.Take(2));
                patternCardsDict[CardPatternType.FullHouse] = fhCards;
            }

            // 가장 높은 족보 적용
            for (int i = 9; i >= 0; i--)
            {
                if (grade[i])
                {
                    CardPatternType type = (CardPatternType)i;
                    CardPattern pattern = new()
                    {
                        patternType = type,
                        cards = patternCardsDict.TryGetValue(type, out var value) ? value : new List<Card>()
                    };
                    
                    Debug.Log($"가능한 패턴 추가 + {type} , count {pattern.cards.Count}");
                    cards_pattern.Add(pattern);
                    /*CurrentPattern = cardPatterns.Find(p => p.PatternType == type);
                    CurrentPatternCards = patternCardsDict.TryGetValue(type, out var value) ? value : new List<Card>();*/

                    /*// 예시: 공격력 등급 수치 설정
                    foreach (var modifier in cardSubmitEffect.gameplayEffect.modifiers)
                    {
                        if (modifier.attrType != GameplayAttr.BasicAttackPower)
                            continue;

                        modifier.gameplayMagnitude.magnitude = gradeNumberCombined[i];
                    }*/

                    break;
                }
            }
        }

    }
}
