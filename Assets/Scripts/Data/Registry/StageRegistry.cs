using TSoft.Data.Stage;
using TSoft.InGame;
using UnityEngine;

namespace TSoft.Data.Registry
{
    [CreateAssetMenu(fileName = "StageRegistry", menuName = "DataRegistry/Stage Registry", order = 0)]
    public class StageRegistry : RegistrySO<StageDataSO>
    {
        public FieldController SpawnNextStage(Transform parent, int currentStage)
        {
            var stageIndex = currentStage % assetDictionary.Count;
            if (!TryGetByIndex(stageIndex, out var stage))
            {
                Debug.LogWarning($"Stage does not exist : search index - {stageIndex}");
                return null;
            }
            
            var field = stage.SpawnStage(parent);
            
            return field;
        }
    }
}
