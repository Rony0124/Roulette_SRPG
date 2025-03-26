using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TSoft;
using TSoft.InGame;
using TSoft.Managers;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace InGame
{
    public class InGameDirector : DirectorBase
    {
        public Action OnPrePlay;

        [Header("Intro")]
        [SerializeField] private float introDuration;
        [SerializeField] private UnityEvent introFeedback;
        
        [Header("PrePlay")]
        [SerializeField] private float prePlayDuration;
        [SerializeField] private UnityEvent prePlayFeedback;
        
        [Header("PostPlay")]
        [SerializeField] private float postPlaySuccessDuration;
        [SerializeField] private float postPlayFailDuration;
        [SerializeField] private UnityEvent postPlaySuccessFeedback;
        [SerializeField] private UnityEvent postPlayFailFeedback;
        
        public List<ControllerBase> Controllers { get; set; }
        
        //life cycle
        private ObservableVar<StageState> currentStageState;
        
        public StageState CurrentStageState => currentStageState.Value;

        protected override void OnDirectorChanged(DirectorBase oldValue, DirectorBase newValue)
        {
            Controllers = FindObjectsOfType<ControllerBase>().ToList();
            
            currentStageState = new ObservableVar<StageState>();
            
            currentStageState.OnValueChanged += (o, n) => OnStageStateChanged(o, n).Forget();;
            
            currentStageState.Value = StageState.Intro;

            foreach (var controller in Controllers)
            {
                controller.Director = this;
            }
        }
        
        private async UniTaskVoid OnStageStateChanged(StageState oldVal, StageState newVal)
        {
            if (oldVal == newVal)
                return;

            Debug.Log($"Current Stage State [{newVal}]");

            if (oldVal != StageState.None)
            {
                //controller cycle 동기화
                foreach (var controller in Controllers)
                {
                    await UniTask.WaitUntil(() => controller.CurrentStageState == oldVal);    
                }
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
            
            //controller cycle 동기화
            foreach (var controller in Controllers)
            {
                controller.OnStageStateChanged(oldVal, newVal).Forget();    
            }
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
            //TODO 보상 시스템 제대로 만들자
            GameSave.Instance.AddGold((int)GameContext.Instance.currentBounty);
            
            SceneManager.LoadScene(Define.Lobby);
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
            introFeedback?.Invoke();
            
            await UniTask.WaitForSeconds(introDuration);
            
            ChangeStageState(StageState.PrePlaying);
        }

        //스테이지 준비
        private async UniTaskVoid StartPrePlaying()
        {
            prePlayFeedback?.Invoke();
            
            await UniTask.WaitForSeconds(prePlayDuration);
            
            ChangeStageState(StageState.Playing);
        }
        
        //스테이지 마무리 준비 (성공)
        private async UniTaskVoid StartPostPlayingSuccess()
        {
            postPlaySuccessFeedback?.Invoke();
            
            await UniTask.WaitForSeconds(postPlaySuccessDuration);
            
            ChangeStageState(StageState.Outro);
        }
        
        //스테이지 마무리 준비 (실패)
        private async UniTaskVoid StartPostPlayingFailed()
        {
            postPlayFailFeedback?.Invoke();
            
            await UniTask.WaitForSeconds(postPlayFailDuration);
            
            ChangeStageState(StageState.Outro);
        }

        #endregion
    }
}
