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
        public static event Action<float> OnDamaged;
        public static event Action<FieldSlot> OnFieldSpawn;
        
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

        private int currentSlotIndex;
        private FieldSlot currentSlot;
        public FieldSlot[] Slots => slots;
        
        public FieldSlot CurrentSlot => currentSlot;
        
        public int CurrentSlotIndex
        {
            get => currentSlotIndex;
            set
            {
                currentSlot = slots[value];
                OnFieldSpawn?.Invoke(currentSlot);
            } 
        }

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
                    if (DataRegistry.Instance.MonsterRegistry.TryGetValue(monsterIds[ranSlotIndex], out var monsterDataSo))
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
                if (DataRegistry.Instance.MonsterRegistry.TryGetValue(bossId, out var bossDataSo))
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
            currentSlot.hp = Math.Max(0, currentSlot.hp - damage);
            Debug.Log("remaining hp : " + currentSlot.hp );
            OnDamaged?.Invoke(currentSlot.hp);

            return currentSlot.hp <= 0;
        }

     
    }
}
