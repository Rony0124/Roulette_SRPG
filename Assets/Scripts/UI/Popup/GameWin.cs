using HF.InGame;
using InGame;
using TMPro;
using TSoft.InGame;
using TSoft.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Popup
{
    public class GameWin : PopupView
    {
        private enum PopupButton
        {
            ButtonConfirm
        }
        
        private enum PopupText
        {
            GoldText
        }
        
        private InGameDirector director;

        private TextMeshProUGUI goldText;

        private void Awake()
        {
            Bind<Button>(typeof(PopupButton));
            Bind<TextMeshProUGUI>(typeof(PopupText));
                
            Get<Button>((int)PopupButton.ButtonConfirm).gameObject.BindEvent(OnButtonClicked);
            goldText = Get<TextMeshProUGUI>((int)PopupText.GoldText);
            
            director = FindObjectOfType<InGameDirector>();
        }

        /*
        protected override void OnActivated()
        {
            base.OnActivated();
            director.Player.CanMoveNextCycle = false;

            goldText.text = GameContext.Instance.currentBounty.ToString();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            director.Player.CanMoveNextCycle = true;
        }
        */

        private void OnButtonClicked(PointerEventData data)
        {
            director.GameFinishSuccess();
        }
    }
}
