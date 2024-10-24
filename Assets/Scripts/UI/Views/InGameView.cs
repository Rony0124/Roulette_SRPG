using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Views
{
    public class InGameView : ViewBase
    {
        private enum InGameButton
        {
            RollButton
        }

        protected override void Init()
        {
            Bind<Button>(typeof(InGameButton));
            Get<Button>((int)InGameButton.RollButton).gameObject.BindEvent(OnButtonClicked);
        }

        protected override void OnActivated()
        {
        }

        protected override void OnDeactivated()
        {
        }
        
        public void OnButtonClicked(PointerEventData data)
        {
            Debug.Log("룰렛 돌려보자");
        }
    }
}
