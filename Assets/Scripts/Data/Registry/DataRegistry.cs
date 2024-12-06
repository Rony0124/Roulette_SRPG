using System;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TSoft.Data.Registry
{
    [CreateAssetMenu(fileName = "DataRegistry")]
    public class DataRegistry : ScriptableSingleton<DataRegistry>
    {
        [SerializeField]
        private AssetReference monsterRegistryRef;

        private MonsterRegistry monsterRegistry; 
        public MonsterRegistry MonsterRegistry => monsterRegistry;
    }
}
