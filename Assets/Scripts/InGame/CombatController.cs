using Cysharp.Threading.Tasks;
using TSoft.Data.Registry;
using UnityEngine;

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
            currentCycleInfo.Round++;
            
            var mIndex = currentCycleInfo.Round;
            currentField.CurrentSlotIndex = mIndex;
            
            Debug.Log("current round" + currentCycleInfo.Round);
            
            await UniTask.WaitForSeconds(1);
        }
    }
}
