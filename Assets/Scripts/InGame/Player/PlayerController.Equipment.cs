using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TSoft;
using TSoft.Data.Card;
using TSoft.Data.Registry;
using TSoft.Item;
using TSoft.Utils;
using UnityEngine;

namespace InGame.Player
{
    public partial class PlayerController
    {
        public Action onJokerChanged;
        
        [Title("Equipments")]
        [SerializeField]
        private List<ArtifactInfo> defaultArtifacts;
        
        public ObservableList<Artifact> CurrentEquippedArtifacts { get; private set; }
        public ObservableList<Joker> CurrentEquippedJokers { get; private set; }
        
        private void InitializeEquipment()
        {
            CurrentEquippedArtifacts = new();
            CurrentEquippedJokers = new();
            
            CurrentEquippedArtifacts.ListChanged += OnEquippedArtifactChanged;
            CurrentEquippedJokers.ListChanged += OnEquippedJokerChanged;
            
            if (!defaultArtifacts.IsNullOrEmpty())
            {
                foreach (var artifact in defaultArtifacts
                             .Where(artifact => artifact != null))
                {
                    AddItem(artifact);
                }
            }

            if (GameSave.Instance.ArtifactEquippedDictionary.Count <= 0)
                return;
            
            foreach (var artifact in GameSave.Instance.ArtifactEquippedDictionary.Values
                         .Select(artifactId => DataRegistry.Instance.ArtifactRegistry.Get(artifactId)))
            {
                AddItem(artifact);
            }
        }

        private void AddItem(ItemInfo itemData)
        {
            switch (itemData)
            {
                case ArtifactInfo:
                    var artifact = new Artifact(itemData);
                    
                    CurrentEquippedArtifacts.Add(artifact);
                    break;
                case CardInfo:
                    var joker = new Joker(itemData);
                    
                    CurrentEquippedJokers.Add(joker);
                    break;
            }
        }
        
        private void OnEquippedArtifactChanged(object sender, ListChangedEventArgs args)
        {
            if(CurrentEquippedArtifacts.Count < 1)
                return;
            
            switch (args.ListChangedType)
            {
                case ListChangedType.ItemAdded :
                    gameplay.AddEffect(CurrentEquippedArtifacts[args.NewIndex].info.effect);
                    break;
                case ListChangedType.ItemDeleted :
                    break;
                case ListChangedType.ItemChanged :
                    break;
            }
        }

        private void OnEquippedJokerChanged(object sender, ListChangedEventArgs args)
        {
            if(CurrentEquippedJokers.Count < 1)
                return;
            
            switch (args.ListChangedType)
            {
                case ListChangedType.ItemAdded :
                    gameplay.AddEffect(CurrentEquippedJokers[args.NewIndex].info.effect);
                    break;
                case ListChangedType.ItemDeleted :
                    break;
                case ListChangedType.ItemChanged :
                    break;
            }
            
            onJokerChanged?.Invoke();
        }
    }
}
