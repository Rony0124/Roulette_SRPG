using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.Utilities;
using TSoft.Data.Card;
using TSoft.InGame.CardSystem;
using TSoft.InGame.GamePlaySystem;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace TSoft.InGame.Player
{
    public class AbilityContainer : MonoBehaviour
    {
        private Gameplay gameplay;
        
        public List<Artifact> artifacts;
        public ObservableList<ArtifactSO> currentArtifacts;
        
        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();

            currentArtifacts = new();
            currentArtifacts.ListChanged += OnSpecialCardsChanged;
        }

        public void Init()
        {
            if (!artifacts.IsNullOrEmpty())
            {
                foreach (var artifact in artifacts)
                {
                    if(artifact == null)
                        continue;
                    
                    currentArtifacts.Add(artifact.ArtifactData);
                }    
            }
        }

        private void OnSpecialCardsChanged(object sender, ListChangedEventArgs args)
        {
            if(currentArtifacts.Count < 1)
                return;
            
            switch (args.ListChangedType)
            {
                case ListChangedType.ItemAdded :
                    ApplyAbility(currentArtifacts[args.NewIndex].effect);
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
