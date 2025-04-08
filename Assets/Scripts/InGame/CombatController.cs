using System;
using Cysharp.Threading.Tasks;
using TSoft.Core;
using TSoft.Data;
using TSoft.Data.Monster;
using TSoft.Data.Registry;
using TSoft.InGame.Player;
using TSoft.Utils;
using UnityEngine;

namespace TSoft.InGame
{
    public class CombatController : Singleton<CombatController>
    {
        public Action<MonsterController> OnMonsterSpawn;
        public Action OnGameStart;
        public Action OnGameEnd;

        private InGameDirector director;

        public InGameDirector Director
        {
            get => director;
            set
            {

                director = value;
                OnDirectorChanged();
            }
        }

        [Header("Player")] 
        [SerializeField] private PlayerController player;
        public PlayerController Player => player;

        [Header("Monster")]
        [SerializeField] private Transform monsterSpawnPoint;
        private MonsterController monster;
        public MonsterController Monster => monster;

        public GamePhase phase = GamePhase.None;

        public int turnCount = 0;
        public int currentTurn = 0; //0 - player turn, 1 - monster turn

        private void OnDirectorChanged()
        {
            MonsterDataSO monsterData = null;
#if UNITY_EDITOR
            if (GameContext.Instance.CurrentMonster == null)
            {
                if (TsDevPreferences.Monster != null)
                {
                    monsterData = TsDevPreferences.Monster;
                }
            }
#endif

            if (monsterData == null)
            {
                monsterData = GameContext.Instance.CurrentMonster;
            }

            monster = monsterData.SpawnMonster(monsterSpawnPoint, Vector3.zero);
            
            OnMonsterSpawn?.Invoke(monster);

            director.CurrentStageState.Value = StageState.Intro;
        }

        public async UniTask GameStart()
        {
            Debug.Log("Game Start");
            await player.GameStart();

            OnGameStart?.Invoke();

            await UniTask.Delay(200);


        }

        public void GameEnd()
        {
            phase = GamePhase.None;

            OnGameEnd?.Invoke();
        }

        public async UniTask StartTurn()
        {
            Debug.Log("Turn Start");
            phase = GamePhase.StartTurn;

            //도트 데미지 같은것 적용

            await UniTask.Delay(200);

            StartMain();
        }

        public void StartNextTurn()
        {
            currentTurn = (currentTurn + 1) % 2;
            if (currentTurn == 0)
            {
                turnCount++;
                Debug.Log($"Current Turn - {turnCount}");
            }

            if (CheckForGameEnd())
            {
                GameEnd();
                return;
            }

            StartTurn().Forget();
        }

        private void StartMain()
        {
            Debug.Log("Turn Main Start");
            phase = GamePhase.Main;

            if (currentTurn == 1)
            {
                monster.AttackPlayer(player);
            }
        }

        public async UniTask EndTurn()
        {
            if (phase != GamePhase.Main)
                return;

            phase = GamePhase.EndTurn;

            //플레이어 상태 효과 감소

            await UniTask.Delay(200);
            StartNextTurn();
        }

        private bool CheckForGameEnd()
        {
            if (monster.IsDead)
            {
                director.GameOver(true);
                return true;
            }

            var currentHeart = player.Gameplay.GetAttr(GameplayAttr.Heart);
            if (currentHeart <= 0)
            {
                director.GameOver(false);
                return true;
            }

            return false;
        }
    }
}
