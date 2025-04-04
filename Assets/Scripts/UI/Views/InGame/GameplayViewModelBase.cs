using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using HF.InGame;
using InGame;
using Sirenix.Utilities;
using UnityEngine;
using PlayerController = HF.InGame.Player.PlayerController;

namespace TSoft.UI.Views.InGame
{
    public class GameplayViewModelBase : ViewModelBase
    {
        private GameplayView View => view as GameplayView;
        private GameplayModel Model => model as GameplayModel;
        
        [SerializeField]
        private GameObject jokerEffectIconPrefab;

        private PlayerController player;
        private List<JokerEffectIcon> jokerEffectIcons = new ();

        private void Start()
        {
            View.DiscardButton.onClick.AddListener(OnDiscardCard);
            View.UseButton.onClick.AddListener(() => OnUseCard().Forget());
            
            /*Model.PlayerHeart.OnValueChanged += OnPlayerHeartChanged;
            Model.PlayerEnergy.OnValueChanged += OnPlayerEnergyChanged;*/
            
            player = Model.Player;
            
            //TODO 이벤트 버스로 변경
            player.onDeckChanged += OnDeckChanged;
           // player.onGameReady += UpdateCardOnGameReady;
            player.onJokerChanged += UpdateJokerEffectIcon;
        }
        
        private void OnPlayerHeartChanged(float oldVal, float newVal)
        {
            /*var maxCount = player.Gameplay.GetAttr(GameplayAttr.MaxHeart);
            View.SetHeartText(newVal, maxCount);*/
        }
        
        private void OnPlayerEnergyChanged(float oldVal, float newVal)
        {
            /*var maxCount = player.Gameplay.GetAttr(GameplayAttr.MaxEnergy);
            View.SetEnergyText(newVal, maxCount);*/
        }
        
        private void OnDiscardCard()
        {
            if(!player.TryDiscardSelectedCard())
                return;
            
            player.DrawCards();
        }
        
        private async UniTaskVoid OnUseCard()
        {
            var dir = GameContext.Instance.CurrentDirector as InGameDirector;
            if (dir != null)
            {
                dir.Combat.EndTurn(CombatController.OwnerPlayerId);
            }
               
            return;
            
            var result = await player.TryUseCardsOnHand();
            if (!result)
                return;
            
            player.DrawCards();
        }
        
        private void OnDeckChanged(int cardNum, int maxCardNum)
        {
            View.SetDeckText(cardNum, maxCardNum);
        }
        
        private void UpdateCardOnGameReady()
        {
            var maxEnergyCount = player.Gameplay.GetAttr(GameplayAttr.MaxEnergy);
            var energyCount = player.Gameplay.GetAttr(GameplayAttr.Energy);
            var maxHeartCount = player.Gameplay.GetAttr(GameplayAttr.MaxHeart);
            var heartCount = player.Gameplay.GetAttr(GameplayAttr.Heart);
            
            View.SetEnergyText(energyCount, maxEnergyCount);
            View.SetHeartText(heartCount, maxHeartCount);
        }

        private void UpdateJokerEffectIcon()
        {
            if (!jokerEffectIcons.IsNullOrEmpty())
            {
                foreach (var icon in jokerEffectIcons)
                {
                    Destroy(icon);
                }
            }

            var items = player.CurrentEquippedJokers;
            foreach (var joker in items)
            {
                var jokerIcon = Instantiate(jokerEffectIconPrefab, View.JokerEffectParent).GetComponent<JokerEffectIcon>();
                jokerIcon.SetIcon(joker);
                jokerEffectIcons.Add(jokerIcon);
            }
        }

        private void OnDestroy()
        {
            player.onDeckChanged -= OnDeckChanged;
         //   player.onGameReady -= UpdateCardOnGameReady;
        }
    }
}
