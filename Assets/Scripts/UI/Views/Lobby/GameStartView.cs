using TSoft.UI.Core;
using TSoft.Utils;
using UnityEngine.Device;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TSoft.UI.Views.Lobby
{
    public class GameStartView : ViewBase
    {
        private enum UIButton
        {
            Restart,
            Continue,
            Exit,
            Option
        }
        
        protected override void OnActivated()
        {
            Bind<Button>(typeof(UIButton));
            
            Get<Button>((int)UIButton.Restart).gameObject.BindEvent(OnRestartClicked);
            Get<Button>((int)UIButton.Continue).gameObject.BindEvent(OnContinueClicked);
            Get<Button>((int)UIButton.Exit).gameObject.BindEvent(OnExitClicked);
            Get<Button>((int)UIButton.Option).gameObject.BindEvent(OnOptionClicked);
        }

        protected override void OnDeactivated()
        {
        }
        
        private void OnRestartClicked(PointerEventData data)
        {
            GameSave.Instance.ClearSaveFile();
            
            SceneManager.LoadScene(Define.StageMap);
        }
        
        private void OnContinueClicked(PointerEventData data)
        {
            SceneManager.LoadScene(Define.StageMap);
        }
        
        private void OnExitClicked(PointerEventData data)
        {
            Application.Quit();
        }
        
        private void OnOptionClicked(PointerEventData data)
        {
            
        }
    }
}
