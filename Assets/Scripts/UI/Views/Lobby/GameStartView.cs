using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TSoft.UI.Views.Lobby
{
    public class GameStartView : ViewBase
    {
        private enum UIButton
        {
            GameStart
        }
        
        protected override void OnActivated()
        {
            Bind<Button>(typeof(UIButton));
            
            Get<Button>((int)UIButton.GameStart).gameObject.BindEvent(OnGameStartClicked);
        }

        protected override void OnDeactivated()
        {
        }
        
        private void OnGameStartClicked(PointerEventData data)
        {
            SceneManager.LoadScene(Define.StageMap);
        }
    }
}
