using System;
using Cysharp.Threading.Tasks;
using TSoft.Data.Monster;
using TSoft.Data.Registry;
using TSoft.UI.Views.InGame;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class CombatController : ControllerBase
    {
        public Action<MonsterController> OnMonsterSpawn;
        
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
                    OnMonsterSpawn?.Invoke(value);
                    
                    currentMonster = value;
                }
            }
        }

        private MonsterDataSO monsterData;

        public UnityEvent onGameFinish;
        
        protected override void InitOnDirectorChanged()
        {
#if UNITY_EDITOR
            if (GameContext.Instance.CurrentNode == null || GameContext.Instance.CurrentNode.Blueprint.monsterId == null)
            {
                if (TsDevPreferences.MonsterId != null)
                {
                    if (DataRegistry.Instance.MonsterRegistry.TryGetValue(TsDevPreferences.MonsterId, out var defaultMonster))
                    { 
                        monsterData = defaultMonster;
                    }    
                }
            }
#endif
            if (monsterData != null) 
                return;
            
            if (DataRegistry.Instance.MonsterRegistry.TryGetValue(GameContext.Instance.CurrentNode.Blueprint.monsterId, out var monsterDataSo))
            { 
                monsterData = monsterDataSo;
            }
        }
        
        protected override async UniTask OnPrePlay()
        {
            CurrentMonster = monsterData.SpawnMonster(transform, Vector3.zero);
            
            await UniTask.WaitForSeconds(1);
        }
    }
}
