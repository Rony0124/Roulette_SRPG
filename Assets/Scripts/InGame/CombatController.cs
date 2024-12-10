using Cysharp.Threading.Tasks;
using TSoft.Data.Registry;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class CombatController : ControllerBase
    {
        public struct CycleInfo
        {
            public int Round;
            public int Stage;
            public bool IsRoundMax => Round >= 5;
            
            public void Reset()
            {
                Round = 0;
                Stage = 0;
            }
        }
        
        //field
        private FieldController currentField;
        //cycle
        private CycleInfo currentCycleInfo;
        
        public FieldController CurrentField => currentField;
        public CycleInfo CurrentCycleInfo => currentCycleInfo;

        public UnityEvent onGameFinish;
        
        protected override void InitOnDirectorChanged()
        {
            currentCycleInfo.Reset();
        }
        
        protected override async UniTask OnPrePlay()
        {
            currentCycleInfo.Round = 0;
            currentCycleInfo.Stage++;
            
            currentField = DataRegistry.Instance.StageRegistry.SpawnNextStage(transform, currentCycleInfo.Stage);
            currentField.SetFieldData(currentCycleInfo);
            
            await UniTask.WaitForSeconds(1);
        }

        protected override async UniTask OnGameReady()
        {
            if (currentCycleInfo.Round > 0)
            {
                onGameFinish?.Invoke();    
            }
            
            await UniTask.WaitForSeconds(1);
            
            currentCycleInfo.Round++;
            
            var mIndex = currentCycleInfo.Round;
            currentField.CurrentSlotIndex = mIndex;
            
            Debug.Log("current round" + currentCycleInfo.Round);
        }
        
        protected override async UniTask OnGameFinishSuccess()
        {
            
            await UniTask.WaitForSeconds(1);
        }
    }
}
