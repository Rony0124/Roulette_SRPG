using System;
using TSoft.Data;
using TSoft.Data.Monster;
using TSoft.Utils;
using UnityEngine;
using PlayerController = InGame.Player.PlayerController;

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
        
        protected override void InitOnDirectorChanged()
        {
            MonsterDataSO monsterData = null;
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
            
            director.prePlayFeedback.AddListener(() => OnPrePlay(monsterData));
        }
        
        private void OnPrePlay(MonsterDataSO monsterData)
        {
            CurrentMonster = monsterData.SpawnMonster(enemySpawnPoint, Vector3.zero);
        }
    }
}
