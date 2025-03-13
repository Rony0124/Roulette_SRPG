using TSoft.Data;
using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.UI.Views.InGame
{
    public class AttackInfoModel : ModelBase
    {
        [SerializeField] private PlayerController player;
        
        public PlayerController Player => player;
    }
}
