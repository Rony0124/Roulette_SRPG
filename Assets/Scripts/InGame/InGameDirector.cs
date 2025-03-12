using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TSoft.Managers;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.InGame
{
    public class InGameDirector : DirectorBase
    {
        public Action OnPrePlay;
        
        //game play
        private PlayerController player;
        private CombatController combat;
        
        //life cycle
        private ObservableVar<StageState> currentStageState;
        
        public PlayerController Player => player;
        public MonsterController CurrentMonster => combat.CurrentMonster;
        
        public StageState CurrentStageState => currentStageState.Value;

        protected override void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue)
        {
            player = FindObjectOfType<PlayerController>();
            combat = FindObjectOfType<CombatController>();
            
            currentStageState = new ObservableVar<StageState>();
            
            currentStageState.OnValueChanged += (o, n) => OnStageStateChanged(o, n).Forget();;
            
            currentStageState.Value = StageState.Intro;

            combat.Director = this;
            player.Director = this;
        }
        
        private async UniTaskVoid OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            if (oldVal == newVal)
                return;

            Debug.Log($"Current Stage State [{newVal}]");

            if (oldVal != StageState.None)
            {
                //controller cycle 동기화
                await UniTask.WaitUntil(() => combat.CurrentStageState == oldVal);
                await UniTask.WaitUntil(() => player.CurrentStageState == oldVal);    
            }
            
            switch (newVal)
            {
                case StageState.Intro:
                    StartIntro().Forget();
                    break;
                case StageState.PrePlaying:
                    StartPrePlaying().Forget();
                    break;
                case StageState.Playing:
                    break;
                case StageState.PostPlayingSuccess:
                    StartPostPlayingSuccess().Forget();
                    break;
                case StageState.PostPlayingFailed:
                    StartPostPlayingFailed().Forget();
                    break;
                case StageState.Outro:
                    break;
                case StageState.Exit:
                    break;
            }
            
            player.OnStageStateChanged(oldVal, newVal).Forget();
            combat.OnStageStateChanged(oldVal, newVal).Forget();
        }
        
        public void ChangeStageState(StageState stageState)
        {
            currentStageState.Value = stageState;
        }

        public void GameOver(bool isSuccess)
        {
            Debug.Log("GameOver");

            if (isSuccess)
            {
                var currentMap = GameContext.Instance.CurrentMap;
                if (currentMap == null) 
                    return;
                
                currentMap.path.Add(GameContext.Instance.CurrentNode.Node.point);
            
                string mapJson = JsonConvert.SerializeObject(currentMap, Formatting.Indented,
                    new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            
                GameSave.Instance.SaveMap(mapJson);
                
                
                ChangeStageState(StageState.PostPlayingSuccess);
            }else
            {
                ChangeStageState(StageState.PostPlayingFailed);
            }
        }

        public void GameFinishSuccess()
        {
            PopupContainer.Instance.ClosePopupUI();
            
            GameSave.Instance.AddGold((int)GameContext.Instance.currentBounty);
            
            SceneManager.LoadScene(Define.StageMap);
        }

        public void GameFinishFail()
        {
            PopupContainer.Instance.ClosePopupUI();

            GameSave.Instance.ClearSaveFile();
            
            SceneManager.LoadScene(Define.Lobby);
        }

        #region Stage

        //입장 인트로
        private async UniTaskVoid StartIntro()
        {
            await UniTask.WaitForSeconds(3);
            
            ChangeStageState(StageState.PrePlaying);
        }

        //스테이지 준비
        private async UniTaskVoid StartPrePlaying()
        {
            OnPrePlay?.Invoke();
            
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.Playing);
        }
        
        //스테이지 마무리 준비 (성공)
        private async UniTaskVoid StartPostPlayingSuccess()
        {
            await UniTask.WaitForSeconds(1);
            
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Win);
        }
        
        //스테이지 마무리 준비 (실패)
        private async UniTaskVoid StartPostPlayingFailed()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.GameOver);
            
            await UniTask.WaitForSeconds(1);
            
            ChangeStageState(StageState.Outro);
        }

        #endregion
    }
}
