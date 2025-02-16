using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TSoft.InGame;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
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
                await appliedEffect.sourceEffect.effect.ApplyEffect(director, this.appliedEffect);
            }
        }

        public async UniTask CheckConditionEffect()
        {
            if (director is null || appliedEffect is null)
                return;

            await appliedEffect.sourceEffect.effect.ApplyEffect(director, appliedEffect);
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
                    await appliedEffect.sourceEffect.effect.ApplyEffect(director, this.appliedEffect);
                }
            }
        }
    }
    
    [Serializable]
    public class ConditionApplierOnCards : ConditionApplier
    {
        private List<PokerCard> currentCheckingCards;
        
        public List<PokerCard> CurrentCheckingCards => currentCheckingCards;
        
        public override async UniTask CheckConditionEffect(InGameDirector director, Gameplay.AppliedGameplayEffect appliedEffect)
        {
            this.director = director;
            this.appliedEffect = appliedEffect;

            currentCheckingCards = director.Player.CurrentPokerCardSelected;
            
            if (context.Evaluate(this))
            {
                await appliedEffect.sourceEffect.effect.ApplyEffect(director, this.appliedEffect);
            }
        }
    }
}
