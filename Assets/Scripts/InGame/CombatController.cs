using Cysharp.Threading.Tasks;
using TSoft.Data.Registry;
using TSoft.Utils;
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
            
            //TODO TEST!!!!!
            public bool IsRoundMax => Round >= 3;
            //public bool IsRoundMax => Round >= 5;
            
            public void Reset()
            {
                Round = 0;
                Stage = 0;
            }
        }

        [SerializeField] private float gameFinishDuration;
        
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

            if (currentField != null)
            {
                Destroy(currentField.gameObject);
            }
            
            currentField = DataRegistry.Instance.StageRegistry.SpawnNextStage(transform, currentCycleInfo.Stage);
            currentField.CurrentCycle = currentCycleInfo;
            
            await UniTask.WaitForSeconds(1);
        }

        protected override async UniTask OnGameReady()
        {
            if (currentCycleInfo.Round > 0)
            {
                if (currentCycleInfo.Round != Define.RewardSlot)
                {
                    onGameFinish?.Invoke();
                    Destroy(currentField.CurrentMonster.gameObject);    
                }
              
            }
            
            await UniTask.WaitForSeconds(1);
            
            currentCycleInfo.Round++;
            currentField.CurrentCycle = currentCycleInfo;
            
            Debug.Log("current round" + currentCycleInfo.Round);
        }
        
        protected override async UniTask OnGameFinishSuccess()
        {
            
            await UniTask.WaitForSeconds(1);
        }
    }
}
