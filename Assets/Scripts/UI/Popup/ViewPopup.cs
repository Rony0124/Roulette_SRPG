using TSoft.Managers;
using TSoft.UI.Core;

namespace TSoft.UI.Popup
{
    public class ViewPopup : ViewBase
    {
        protected override void OnActivated()
        {
            PopupContainer.Instance.SetCanvas(gameObject);
        }

        protected override void OnDeactivated()
        {
        }

        public virtual void ClosePopupUI()
        {
            PopupContainer.Instance.ClosePopupUI(this);
        }
    }
}
