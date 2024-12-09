using TSoft.InGame;
using UnityEngine;

namespace TSoft.Data.Stage
{
    [CreateAssetMenu(fileName = "StageData", menuName = "Data/Stage Data", order = 0)]
    public class StageDataSO : DataSO
    {
        [SerializeField]
        private GameObject fieldPrefab;

        [SerializeField]
        private StageData data;

        public FieldController SpawnStage(Transform parent)
        {
            var fieldObj = Instantiate(fieldPrefab, parent);
            var field = fieldObj.GetComponent<FieldController>();
            
            field.SpawnField(data);

            return field;
        }
    }
}
