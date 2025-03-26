using System;
using Cysharp.Threading.Tasks;
using TSoft.Data;
using TSoft.Data.Monster;
using TSoft.InGame;
using TSoft.InGame.Player;
using TSoft.Utils;
using UnityEngine;

namespace InGame
{
    public class CombatController : ControllerBase
    {
        public Action<MonsterController> OnMonsterSpawn;

        [Header("Player")] 
        [SerializeField] private PlayerController player;

        [Header("Enemy")] 
        [SerializeField] private Transform enemySpawnPoint;
        
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
        
        
        protected override void InitOnDirectorChanged()
        {
#if UNITY_EDITOR
            if (GameContext.Instance.CurrentNode == null || GameContext.Instance.CurrentNode.Blueprint.monsterData.Id == RegistryId.Null)
            {
                if (TsDevPreferences.Monster != null && TsDevPreferences.overrideMonster)
                {
                    monsterData = TsDevPreferences.Monster;
                }
            }
#endif
            if (monsterData != null) 
                return;
            
            monsterData = GameContext.Instance.CurrentMonster;
        }
        
        protected override async UniTask OnPrePlay()
        {
            CurrentMonster = monsterData.SpawnMonster(enemySpawnPoint, Vector3.zero);
            director.Controllers.Add(currentMonster);
            
            CurrentMonster.Director = director;
            CurrentMonster.OnStageStateChanged(StageState.None, currentStageState).Forget();
            
            director.Controllers.Add(currentMonster);

            await UniTask.CompletedTask;
        }
    }
}
