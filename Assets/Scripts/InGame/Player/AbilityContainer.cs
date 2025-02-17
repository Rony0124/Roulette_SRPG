using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.Utilities;
using TSoft.Data.Card;
using TSoft.InGame.GamePlaySystem;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame.Player
{
    public class AbilityContainer : MonoBehaviour
    {
        private Gameplay gameplay;
        
        public List<ArtifactSO> defaultArtifacts;
        public ObservableList<ArtifactSO> currentArtifacts;
        
        private void Awake()
        {
            gameplay = GetComponent<Gameplay>();

            currentArtifacts = new();
            currentArtifacts.ListChanged += OnArtifactsChanged;
        }

        public void Init()
        {
            if (!defaultArtifacts.IsNullOrEmpty())
            {
                foreach (var artifact in defaultArtifacts)
                {
                    if(artifact == null)
                        continue;
                    
                    currentArtifacts.Add(artifact);
                }    
            }
        }

        private void OnArtifactsChanged(object sender, ListChangedEventArgs args)
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
            gameplay.AddEffect(so);
        }
    }
}
