using TSoft.Data.Registry;
using TSoft.InGame;
using UnityEngine;

namespace TSoft.Data.Stage
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Data/Stage Data", order = 0)]
    public class MonsterData : DataSO
    {
        [SerializeField]
        private GameObject fieldPrefab;

        /*public FieldController SpawnStage(Transform parent)
        {
            var fieldObj = Instantiate(fieldPrefab, parent);
            var field = fieldObj.GetComponent<FieldController>();
            
            field.SpawnField(data);

            return field;
        }*/
        
        public MonsterController SpawnMonster(Transform parent)
        {
            MonsterController monster = null;
            if (DataRegistry.Instance.MonsterRegistry.TryGetValue(registryId, out var monsterDataSo))
            {
                var pos = Vector3.zero;
                monster = monsterDataSo.SpawnMonster(parent, pos);
            }

            return monster;
        }
    }
}
