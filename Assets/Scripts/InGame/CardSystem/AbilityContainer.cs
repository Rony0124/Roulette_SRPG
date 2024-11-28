using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using TSoft.InGame.GamePlaySystem;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame.CardSystem
{
    public class AbilityContainer : MonoBehaviour
    {
        public List<SpecialCard> SpecialCards;
        public ObservableList<SpecialCardData> CurrentSpecialCards;

        private Gameplay gameplay;
        
        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();

            CurrentSpecialCards = new();
            CurrentSpecialCards.ListChanged += OnSpecialCardsChanged;
        }

        private void Start()
        {
            foreach (var card in SpecialCards)
            {
                //gameplay.ApplyEffectSelf(card.cardData.Effect);
                CurrentSpecialCards.Add(card.cardData);
            }
            
            /*foreach (var specialCard in CurrentSpecialCards)
            {
                gameplay.ApplyEffectSelf(specialCard.Effect);
            }*/
        }

        private void OnSpecialCardsChanged(object sender, ListChangedEventArgs args)
        {
            switch (args.ListChangedType)
            {
                case ListChangedType.ItemAdded :
                    ApplyAbility(CurrentSpecialCards[args.NewIndex].Effect);
                    break;
                case ListChangedType.ItemDeleted :
                    break;
                case ListChangedType.ItemChanged :
                    break;
                    
            }
        }

        public void ApplyAbility(GameplayEffectSO so)
        {
            gameplay.ApplyEffectSelf(so);
        }
    }
}
