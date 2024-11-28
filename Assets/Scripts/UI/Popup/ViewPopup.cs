using TSoft.Managers;
using TSoft.UI.Core;

namespace TSoft.UI.Popup
{
    public class ViewPopup : ViewBase
    {

        protected override void OnActivated()
        {
            UIManager.Instance.SetCanvas(gameObject);
        }

        protected override void OnDeactivated()
        {
        }

        public virtual void ClosePopupUI()
        {
            UIManager.Instance.ClosePopupUI(this);
        }
    }
}
