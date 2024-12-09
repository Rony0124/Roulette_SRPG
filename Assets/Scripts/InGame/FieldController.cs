using System;
using System.Collections.Generic;
using TSoft.Data;
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
        }
        
        [Header("Slots")]
        [SerializeField] 
        private FieldSlot[] slots;

        private int currentSlotIndex;

        public FieldSlot[] Slots => slots;
        public int CurrentSlotIndex => currentSlotIndex;

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
                    if (GameContext.Instance.MonsterRegistry.TryGetValue(monsterIds[ranSlotIndex], out var monsterDataSo))
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
                    bossDataSo.SpawnMonster(slots[BossSlot].self, Vector3.zero);
                }        
            }
            
        }
    }
}
