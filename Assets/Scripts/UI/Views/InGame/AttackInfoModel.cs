using TSoft.Data;
using UnityEngine;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.UI.Views.InGame
{
    public class AttackInfoModel : ModelBase
    {
        [SerializeField] private PlayerController player;
        
        public PlayerController Player => player;
    }
}
