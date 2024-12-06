using TSoft.InGame;
using UnityEngine;

namespace TSoft.Data.Monster
{
    [CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster Data", order = 0)]
    public class MonsterDataSO : DataSO
    {
        [SerializeField]
        private GameObject monsterPrefab;
        [SerializeField]
        private MonsterData monsterData;

        public MonsterController SpawnMonster(Transform parent, Vector3 position)
        {
            var monsterObj = Instantiate(monsterPrefab, parent);
            
            var monsterController = monsterObj.GetComponent<MonsterController>();
            monsterController.Data = monsterData;
            
            var transform = monsterObj.transform;
            transform.localPosition = position;

            return monsterController;
        }
    }
}
