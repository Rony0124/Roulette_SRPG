using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TSoft.InGame;
using TSoft.InGame.GamePlaySystem;

namespace TSoft.Data.Condition
{
    public class SelectedCardCondition : ConditionSO
    {
        public enum OddEven
        {
            Odd,
            Even
        }
        
        public enum CardConditionType
        {
            OddEven,
            CardPattern,
            CardType,
            NumberCombination
        }
        
        public CardConditionType conditionType;
            
        [ShowIf("conditionType", CardConditionType.CardPattern)]
        public CardPatternType cardPatternType;

        [ShowIf("conditionType", CardConditionType.OddEven)]
        public OddEven oddEven;
        
        [ShowIf("conditionType", CardConditionType.NumberCombination)]
        public int[] numberCombination;
        
        [ShowIf("conditionType", CardConditionType.CardType)]
        public CardType cardType;
        
        public override async UniTask<bool> CheckCondition(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            var selectedCards = director.Player.CurrentPokerCardSelected;
            if (selectedCards.IsNullOrEmpty())
                return false;

            foreach (var selectedCard in selectedCards)
            {
                switch (conditionType)
                {
                    case CardConditionType.CardType:
                        if (selectedCard.cardData.type == cardType)
                        {
                            await appliedEffect.sourceEffect.effect.ApplyEffect(director, appliedEffect);
                        }
                        break;
                    case CardConditionType.OddEven:
                        if (selectedCard.cardData.number % 2 == 0 && oddEven == OddEven.Even)
                        {
                            await appliedEffect.sourceEffect.effect.ApplyEffect(director, appliedEffect);
                        }else if (selectedCard.cardData.number % 2 != 0 && oddEven == OddEven.Odd)
                        {
                            await appliedEffect.sourceEffect.effect.ApplyEffect(director, appliedEffect);
                        }
                        break;
                    case CardConditionType.NumberCombination:
                        if (numberCombination.Contains(selectedCard.cardData.number))
                        {
                            await appliedEffect.sourceEffect.effect.ApplyEffect(director, appliedEffect);
                        }
                        break;
                }
            }
            
            if (director.Player.CurrentPattern.PatternType == cardPatternType)
            {
                await appliedEffect.sourceEffect.effect.ApplyEffect(director, appliedEffect);   
            }
            
            return false;
        }
    }
}
