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
        }
        
        [Header("Monster")]
        [SerializeField] private List<DataRegistryIdSO> monsterIds;
        
        [Header("Boss")]
        [SerializeField] private DataRegistryIdSO bossId;
        
        [Header("Slots")]
        [SerializeField] private FieldSlot[] slots;

        private int currentSlotIndex;

        private const int MonsterSlotMax = 3;
        private const int RewardSlot = 4;
        private const int BossSlot = 5;

        public void SpawnField()
        {
            //몬스터 소환
            for (var i = 0; i < MonsterSlotMax; i++)
            {
                var ranSlotIndex = Random.Range(0, monsterIds.Count);
                var ranSlotPosIndex = 1;
                if (slots[i].slotPositions.Count > 0)
                {
                    ranSlotPosIndex = Random.Range(1, slots[i].slotPositions.Count);    
                }

                for (var j = 0; j < ranSlotPosIndex; j++)
                {
                    if(DataRegistry.instance == null)
                        return;
                    
                    if (DataRegistry.instance.MonsterRegistry.TryGetValue(monsterIds[ranSlotIndex], out var monsterDataSo))
                    {
                        Vector3 pos = slots[i].slotPositions[j].position;
                        
                        if (ranSlotIndex == 1)
                        {
                            pos = Vector3.zero;
                        }
                        
                        monsterDataSo.SpawnMonster(slots[i].self, pos);
                    }    
                }
            }
            
            //리워드
            Debug.Log("리워드 필드 소환");

            if (bossId != null)
            {
                //보스 소환
                if (DataRegistry.instance.MonsterRegistry.TryGetValue(bossId, out var bossDataSo))
                {
                    bossDataSo.SpawnMonster(slots[BossSlot].self, Vector3.zero);
                }        
            }
            
        }
    }
}
