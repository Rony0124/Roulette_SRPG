using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using InGame;
using TSoft.InGame;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.Data.Condition
{
    [Serializable]
    public class ConditionApplier
    {
        [HideInInspector]
        public InGameDirector director;
        [HideInInspector]
        public Gameplay.AppliedGameplayEffect appliedEffect;

        [SerializeField] 
        public ConditionContext context;

        public virtual async UniTask CheckConditionEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            this.director = director;
            this.appliedEffect = appliedEffect;

            if (context.Evaluate(this))
            {
                await appliedEffect.sourceEffect.gameplayEffect.effect.ApplyEffect(director, this.appliedEffect);
            }
            else if (appliedEffect.sourceEffect.hasUnsatisfiedEffect)
            {
                await appliedEffect.sourceEffect.unsatisfiedEffect.effect.ApplyEffect(director, this.appliedEffect);
            }
        }
    }
    
    [Serializable]
    public class ConditionApplierOnCardsEach : ConditionApplier
    {
        private PokerCard currentCheckingCard;
        public PokerCard CurrentCheckingCard => currentCheckingCard;
        
        public override async UniTask CheckConditionEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            this.director = director;
            this.appliedEffect = appliedEffect;

            var currentSelectedCards = director.Player.CurrentPokerCardSelected;

            foreach (var selectedCard in currentSelectedCards)
            {
                currentCheckingCard = selectedCard;
                
                if (context.Evaluate(this))
                {
                    await appliedEffect.sourceEffect.gameplayEffect.effect.ApplyEffect(director, this.appliedEffect);
                }
                else if (appliedEffect.sourceEffect.hasUnsatisfiedEffect)
                {
                    await appliedEffect.sourceEffect.unsatisfiedEffect.effect.ApplyEffect(director, this.appliedEffect);
                }

                await UniTask.Delay(10);
            }
        }
    }
    
    [Serializable]
    public class ConditionApplierOnCards : ConditionApplier
    {
        private PlayerController.CardPattern currentPattern;
        
        public PlayerController.CardPattern CurrentPattern => currentPattern;
        
        private List<PokerCard> currentCards;
        public List<PokerCard> CurrentCards => currentCards;
        
        public override async UniTask CheckConditionEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            this.director = director;
            this.appliedEffect = appliedEffect;

            currentPattern = director.Player.CurrentPattern;
            currentCards = director.Player.CurrentPokerCardSelected;
            
            if (context.Evaluate(this))
            {
                await appliedEffect.sourceEffect.gameplayEffect.effect.ApplyEffect(director, this.appliedEffect);
            }
            else if (appliedEffect.sourceEffect.hasUnsatisfiedEffect)
            {
                await appliedEffect.sourceEffect.unsatisfiedEffect.effect.ApplyEffect(director, this.appliedEffect);
            }
        }
    }
}
