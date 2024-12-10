using System;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TSoft.Data.Registry
{
    [CreateAssetMenu(fileName = "DataRegistry")]
    public class DataRegistry : ScriptableSingleton<DataRegistry>
    {
        [SerializeField] private AssetReference monsterRegistryRef;
        [SerializeField] private AssetReference stageRegistryRef;
        
        private StageRegistry stageRegistry;
        private MonsterRegistry monsterRegistry;
        
        public StageRegistry StageRegistry => stageRegistry;
        public MonsterRegistry MonsterRegistry => monsterRegistry;

        public async UniTask Load()
        {
            monsterRegistry  = await monsterRegistryRef.LoadAssetAsync<MonsterRegistry>();
            stageRegistry  = await monsterRegistryRef.LoadAssetAsync<StageRegistry>();
        }
    }
}
