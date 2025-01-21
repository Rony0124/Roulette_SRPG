using Cysharp.Threading.Tasks;
using TSoft.Data.Monster;
using TSoft.Data.Registry;
using TSoft.Data.Stage;
using TSoft.UI.Views.InGame;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class CombatController : ControllerBase
    {
        /*public struct CycleInfo
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
        }*/

        //view
        [SerializeField]
        private BackgroundView bgView;
        [SerializeField]
        private FieldInfoView infoView;
        
        [SerializeField] private float gameFinishDuration;
        
        //monster
        private MonsterController currentMonster;

        public MonsterController CurrentMonster
        {
            get => currentMonster;
            set
            {
                if (value != null)
                {
                    infoView.OnMonsterSpawn?.Invoke(value);
                    bgView.OnMonsterSpawn?.Invoke(value.Data.monsterType);
                    
                    currentMonster = value;
                }
            }
        }

        private MonsterDataSO monsterData;

        public UnityEvent onGameFinish;
        
        protected override void InitOnDirectorChanged()
        {
          if (DataRegistry.Instance.MonsterRegistry.TryGetValue(GameContext.Instance.CurrentNode.monsterId, out var monsterDataSo))
          {
              monsterData = monsterDataSo;
          }
        }
        
        protected override async UniTask OnPrePlay()
        {
            CurrentMonster = monsterData.SpawnMonster(transform, Vector3.zero);
            
            await UniTask.WaitForSeconds(1);
        }

        protected override async UniTask OnGameReady()
        {
            await UniTask.WaitForSeconds(1);
        }
        
        protected override async UniTask OnGameFinishSuccess()
        {
            
            await UniTask.WaitForSeconds(1);
        }
    }
}
