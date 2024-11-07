using System;
using TSoft.InGame.Roulette;
using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSoft.UI.Views
{
    public class RouletteView : ViewBase
    {
        private enum RouletteButton
        {
            RollButton,
            LeftButton,
            RightButton
        }

        public Action OnRollClick;
        
        private RouletteChipSet chipSet;

        private void Awake()
        {
            chipSet = GetComponentInChildren<RouletteChipSet>();
            
            Bind<Button>(typeof(RouletteButton));
            
            Get<Button>((int)RouletteButton.RollButton).gameObject.BindEvent(OnRollButtonClicked);
            Get<Button>((int)RouletteButton.LeftButton).gameObject.BindEvent(OnLeftButtonClicked);
            Get<Button>((int)RouletteButton.RightButton).gameObject.BindEvent(OnRightButtonClicked);
        }

        protected override void OnActivated() { }

        protected override void OnDeactivated() { }
        
        private void OnRollButtonClicked(PointerEventData data)
        {
            OnRollClick?.Invoke();
        }
        
        private void OnLeftButtonClicked(PointerEventData data)
        {
            chipSet.ChangeChip(-1);
        } 
        
        private void OnRightButtonClicked(PointerEventData data)
        {
            chipSet.ChangeChip(1);
        }
    }
}
