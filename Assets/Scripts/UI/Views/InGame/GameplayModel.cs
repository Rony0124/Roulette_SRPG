using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.UI.Views.InGame
{
    public class GameplayModel : ModelBase
    {
        //Play
        [SerializeField]
        private PlayerController player;

        public PlayerController Player => player;
    }
}
