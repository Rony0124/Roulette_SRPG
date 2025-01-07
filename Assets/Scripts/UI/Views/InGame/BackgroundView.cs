using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.InGame
{
    public class BackgroundView : ViewBase
    {
        private enum BgImage
        {
            Background
        }

        private Image bg;
        
        private void Awake()
        {
            Bind<Image>(typeof(BgImage));
            
            bg = Get<Image>((int)BgImage.Background);
        }
        
        protected override void OnActivated()
        {
        }

        protected override void OnDeactivated()
        {
        }

        public void SetBackground(Sprite background)
        {
            bg.sprite = background;
        }
    }
}
