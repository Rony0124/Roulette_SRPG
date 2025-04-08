using System;
using TSoft.InGame;
using TSoft.Utils;
using UnityEngine;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.UI.Views.InGame
{
    public class GameplayModel : ModelBase
    {
        [SerializeField]
        private PlayerController player;
        
        public PlayerController Player => player;

        public ObservableVar<float> PlayerHeart => player.Gameplay.GetAttrVar(GameplayAttr.Heart);
        public ObservableVar<float> PlayerEnergy => player.Gameplay.GetAttrVar(GameplayAttr.Energy);

        private void OnDestroy()
        {
            PlayerHeart?.Dispose();
            PlayerEnergy?.Dispose();
        }
    }
}
