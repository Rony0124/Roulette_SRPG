using UnityEditor;
using UnityEngine;

namespace TSoft.Data.Registry
{
    [CreateAssetMenu(fileName = "DataRegistry")]
    public class DataRegistry : ScriptableSingleton<DataRegistry>
    {
        [SerializeField]
        private MonsterRegistry monsterRegistry;
        
        public MonsterRegistry MonsterRegistry => monsterRegistry;
    }
}
