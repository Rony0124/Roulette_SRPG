using UnityEngine;

namespace TSoft.Data
{
    public class DataSO : ScriptableObject
    {
        [SerializeField]
        protected DataRegistryIdSO registryId;
        
        public DataRegistryIdSO RegistryId => registryId;
        
        public bool IsEqual(DataRegistryIdSO id)
        {
            return id == registryId;
        }
    }
}
