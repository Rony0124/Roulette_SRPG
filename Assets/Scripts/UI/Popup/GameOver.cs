using InGame;
using TSoft.InGame;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup
{
    public class GameOver : PopupView
    {
        private enum PopupButton
        {
            ButtonConfirm
        }
        
        private InGameDirector director;
        
        private void Awake()
        {
            Bind<Button>(typeof(PopupButton));
            
            Get<Button>((int)PopupButton.ButtonConfirm).gameObject.BindEvent(OnButtonClicked);
            
            director = FindObjectOfType<InGameDirector>();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            director.Player.CanMoveNextCycle = false;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            director.Player.CanMoveNextCycle = true;
        }

        private void OnButtonClicked(PointerEventData data)
        {
            director.GameFinishFail();
        }
    }
}
