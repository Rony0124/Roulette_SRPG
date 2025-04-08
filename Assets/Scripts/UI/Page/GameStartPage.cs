using Cysharp.Threading.Tasks;
using TSoft;
using TSoft.Save;
using UI.Views.Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Page
{
    public class GameStartPage : UIPage
    {
        private enum UIButton
        {
            Restart,
            Continue,
            Exit,
            Option
        }
        
        [SerializeField] private ObjectActivator activator;
        
        void Awake()
        {
            Bind<Button>(typeof(UIButton));
            
            Get<Button>((int)UIButton.Restart).onClick.AddListener(() => OnRestartClicked().Forget());
            Get<Button>((int)UIButton.Continue).onClick.AddListener(() => OnContinueClicked().Forget());
            Get<Button>((int)UIButton.Exit).onClick.AddListener(OnExitClicked);
            Get<Button>((int)UIButton.Option).onClick.AddListener(OnOptionClicked);
        }
        
        private async UniTaskVoid OnRestartClicked()
        {
            GameSave.Instance.ClearSaveFile();

            await activator.ReturnActivatedCards();
            
            var view = CurrentView as GameStartView;
            view.MoveToNextPage();
        }
        
        private async UniTaskVoid OnContinueClicked()
        {
            await activator.ReturnActivatedCards();
            
            var view = CurrentView as GameStartView;
            view.MoveToNextPage();
            //SceneManager.LoadScene(Define.StageMap);
        }
        
        private void OnExitClicked()
        {
            Application.Quit();
        }
        
        private void OnOptionClicked()
        {
            
        }
    }
}
