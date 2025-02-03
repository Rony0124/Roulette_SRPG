using TSoft.Managers;
using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.Store
{
    public class InventoryView : ViewBase
    {
        private enum InventoryButton
        {
            SkillButton,
            JokerButton,
            ArtifactButton
        }
        
        protected override void OnActivated()
        {
        }

        protected override void OnDeactivated()
        {
        }
        
        private void Awake()
        {
            Bind<Button>(typeof(InventoryButton));
            
            Get<Button>((int)InventoryButton.SkillButton).onClick.AddListener(OnSkillClicked);
            Get<Button>((int)InventoryButton.JokerButton).onClick.AddListener(OnJokerClicked);
            Get<Button>((int)InventoryButton.ArtifactButton).onClick.AddListener(OnArtifactClicked);
        }
        
        private void OnSkillClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Skill);
        }
        
        private void OnJokerClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Joker);
        }
        
        private void OnArtifactClicked()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Artifact);
        }
    }
}
