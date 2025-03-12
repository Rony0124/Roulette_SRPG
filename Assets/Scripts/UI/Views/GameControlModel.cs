using TSoft.InGame.Player;
using UnityEngine;

namespace TSoft.UI.Views
{
    public class GameControlModel : Model
    {
        //Play
        [SerializeField]
        private PlayerController player;

        public PlayerController Player => player;
    }
}
