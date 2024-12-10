using System;
using System.Collections.Generic;
using TSoft.Data;
using TSoft.Data.Registry;
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
            public List<Transform> slotPositions;
            [NonSerialized]
            public List<MonsterController> monsters;
            public int hp;
        }
        
        [Header("Slots")]
        [SerializeField] 
        private FieldSlot[] slots;
        public FieldSlot[] Slots => slots;
        
        public int CurrentSlotIndex { get; set; }

        public FieldSlot CurrentSlot => slots[CurrentSlotIndex];

        private const int MonsterSlotMax = 3;
        private const int RewardSlot = 4;
        private const int BossSlot = 5;
        
        public void SpawnField(Data.Stage.StageData stageData)
        {
            var monsterIds = stageData.monsterIds;
            var bossId = stageData.bossId;
            
            //몬스터 소환
            for (var i = 0; i <= MonsterSlotMax; i++)
            {
                slots[i].monsters = new List<MonsterController>();
                
                var ranSlotIndex = Random.Range(0, monsterIds.Length);
                var ranSlotPosIndex = 1;
                if (slots[i].slotPositions.Count > 0)
                {
                    ranSlotPosIndex = Random.Range(1, slots[i].slotPositions.Count);    
                }

                for (var j = 0; j < ranSlotPosIndex; j++)
                {
                    if (DataRegistry.instance.MonsterRegistry.TryGetValue(monsterIds[ranSlotIndex], out var monsterDataSo))
                    {
                        var pos = Vector3.zero;
                        if (ranSlotIndex > 1)
                        {
                            pos = slots[i].slotPositions[j].position;
                        }
                        
                        var monster = monsterDataSo.SpawnMonster(slots[i].self, pos);
                        slots[i].monsters.Add(monster); 
                    }    
                }
            }
            
            //리워드
            Debug.Log("리워드 필드 소환");

            if (bossId != null)
            {
                //보스 소환
                if (GameContext.Instance.MonsterRegistry.TryGetValue(bossId, out var bossDataSo))
                {
                    var boss = bossDataSo.SpawnMonster(slots[BossSlot].self, Vector3.zero);
                    slots[BossSlot].monsters.Add(boss); 
                }        
            }
        }

        public void SetFieldData(CombatController.CycleInfo cycle)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];

                if (i == BossSlot)
                {
                    slot.hp *= (int)(cycle.Stage * 1.5);
                }
                else
                {
                    slot.hp *= cycle.Stage;
                }
            }
        }

        public bool TakeDamage(int damage)
        {
            CurrentSlot.hp -= damage;
            Debug.Log("remaining hp : " +CurrentSlot.hp );

            return CurrentSlot.hp < 0;
        }

     
    }
}
