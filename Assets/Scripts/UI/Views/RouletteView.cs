using System;
using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Views
{
    public class RouletteView : ViewBase
    {
        private enum RouletteButton
        {
            RollButton
        }

        public Action OnRollClick;

        private void Awake()
        {
            Bind<Button>(typeof(RouletteButton));
            Get<Button>((int)RouletteButton.RollButton).gameObject.BindEvent(OnRollButtonClicked);
        }
        
        private void OnRollButtonClicked(PointerEventData data)
        {
            OnRollClick?.Invoke();
        }

        protected override void OnActivated() { }

        protected override void OnDeactivated() { }
    }
}
