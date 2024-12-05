using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.Utilities;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public class AbilityContainer : MonoBehaviour
    {
        private Gameplay gameplay;
        
        public List<SpecialCard> SpecialCards;
        public ObservableList<SpecialCardData> CurrentSpecialCards;
        
        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();

            CurrentSpecialCards = new();
            CurrentSpecialCards.ListChanged += OnSpecialCardsChanged;
        }

        private void Start()
        {
            if (!SpecialCards.IsNullOrEmpty())
            {
                foreach (var card in SpecialCards)
                {
                    if(card == null)
                        continue;
                    
                    //gameplay.ApplyEffectSelf(card.cardData.Effect);
                    CurrentSpecialCards.Add(card.cardData);
                }    
            }
            
            /*foreach (var specialCard in CurrentSpecialCards)
            {
                gameplay.ApplyEffectSelf(specialCard.Effect);
            }*/
        }

        private void OnSpecialCardsChanged(object sender, ListChangedEventArgs args)
        {
            if(CurrentSpecialCards.Count < 1)
                return;
            
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
