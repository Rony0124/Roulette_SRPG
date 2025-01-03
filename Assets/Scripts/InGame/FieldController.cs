using System;
using TSoft.Data.Registry;
using TSoft.UI.Views.InGame;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TSoft.InGame
{
    public class FieldController : MonoBehaviour
    {
        [Serializable]
        public class FieldSlot
        {
            public Transform self;
            [NonSerialized]
            public MonsterController monster;
        }
        
        [Header("Slots")]
        [SerializeField] 
        private FieldSlot[] slots;
        
        //refactor ui referencing
        private FieldInfoView view => FindObjectOfType<FieldInfoView>();
        
        private int currentSlotIndex;
        private FieldSlot currentSlot;
        
        public FieldSlot[] Slots => slots;
        public FieldSlot CurrentSlot => currentSlot;
        
        public int CurrentSlotIndex
        {
            get => currentSlotIndex;
            set
            {
                var slotVal = value - 1;
                currentSlot = slots[slotVal];
                if (slotVal == RewardSlot)
                {
                    Debug.Log("fldnjem tmffhtdlfkrh???");
                    // view.OnRewardSpawn?.Invoke();
                }
                else
                {
                    view.OnMonsterSpawn?.Invoke(currentSlot);    
                }
            } 
        }

        //test
        private const int MonsterSlotMax = 0;
        private const int RewardSlot = 1;
        private const int BossSlot = 2;

        public void SpawnField(Data.Stage.StageData stageData)
        {
            var monsterIds = stageData.monsterIds;
            var bossId = stageData.bossId;
            
            //몬스터 소환
            for (var i = 0; i <= MonsterSlotMax; i++)
            {
                var ranSlotIndex = Random.Range(0, monsterIds.Length);

                if (DataRegistry.Instance.MonsterRegistry.TryGetValue(monsterIds[ranSlotIndex], out var monsterDataSo))
                {
                    var pos = Vector3.zero;
                    var monster = monsterDataSo.SpawnMonster(slots[i].self, pos);
                    slots[i].monster = monster; 
                }
            }
            
            //리워드
            Debug.Log("리워드 필드 소환");

            if (bossId != null)
            {
                //보스 소환
                if (DataRegistry.Instance.MonsterRegistry.TryGetValue(bossId, out var bossDataSo))
                {
                    var boss = bossDataSo.SpawnMonster(slots[BossSlot].self,  Vector3.zero);
                    slots[BossSlot].monster = boss;
                }        
            }
        }

        public void SetFieldData(CombatController.CycleInfo cycle)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                
                if(i == RewardSlot)
                    continue;
                
                if(slot.monster is null)
                    continue;
                
                var currentHp = slot.monster.GamePlay.GetAttr(GameplayAttr.Heart);
                if (i == BossSlot)
                {
                    currentHp *= (int)(cycle.Stage * 1.5);
                }
                else
                {
                    currentHp *= cycle.Stage;
                }
                
                slot.monster.GamePlay.SetAttr(GameplayAttr.Heart, currentHp);
            }
        }

        public bool TakeDamage(int damage)
        {
            var currentHp = currentSlot.monster.GamePlay.GetAttr(GameplayAttr.Heart);
            currentHp = Math.Max(0, currentHp - damage);
            Debug.Log("remaining hp : " + currentHp );
            
            currentSlot.monster.GamePlay.SetAttr(GameplayAttr.Heart, currentHp);
            view.OnDamaged?.Invoke(currentHp);

            return currentHp <= 0;
        }
    }
}
