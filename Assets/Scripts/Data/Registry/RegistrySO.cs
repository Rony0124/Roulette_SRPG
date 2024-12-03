using UnityEngine;
using UnityEngine.Rendering;

namespace TSoft.Data.Registry
{
    public class RegistrySO<TData> : ScriptableObject
        where TData : DataSO
    {
        [SerializeField]
        public SerializedDictionary<DataRegistryIdSO, TData> assetDictionary;

        public TData Get(DataRegistryIdSO idSo)
        {
            return assetDictionary.TryGetValue(idSo, out var data) ? data : null;
        }

        public bool TryGetValue(DataRegistryIdSO idSo, out TData data)
        {
            return assetDictionary.TryGetValue(idSo, out data);
        }
    }
}
