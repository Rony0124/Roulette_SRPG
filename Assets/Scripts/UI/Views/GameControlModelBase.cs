using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.UI.Views
{
    public class GameControlModel : ModelBase
    {
        //Play
        [SerializeField]
        private PlayerController player;

        public PlayerController Player => player;
    }
}
