using TSoft;
using TSoft.InGame.Player;
using UnityEngine;

namespace UI.Views.InGame
{
    public class AttackInfoModel : ModelBase
    {
        [SerializeField] private PlayerController player;
        
        public PlayerController Player => player;
    }
}
