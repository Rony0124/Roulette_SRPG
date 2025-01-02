using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace TSoft.Data.Registry
{
    public class RegistrySO<TData> : ScriptableObject
        where TData : DataSO
    {
        [SerializeField]
        private SerializedDictionary<DataRegistryIdSO, TData> assetDictionary;

        public int Count => assetDictionary.Count;
        public TData Get(DataRegistryIdSO key)
        {
            return assetDictionary.TryGetValue(key, out var data) ? data : null;
        }

        public bool TryGetValue(DataRegistryIdSO key, out TData data)
        {
            return assetDictionary.TryGetValue(key, out data);
        }

        public bool TryGetDataByIndex(int index, out TData data)
        {
            var list = assetDictionary.Keys.ToList();
            var key = list[index];
            return assetDictionary.TryGetValue(key, out data);
        }
        
        public bool TryGetKvpByIndex(int index, out KeyValuePair<Guid, TData> result)
        {
            result = default;
            
            var list = assetDictionary.Keys.ToList();
            var key = list[index];
            
            foreach (var kvp in assetDictionary)
            {
                if (kvp.Key == key)
                {
                    result = new KeyValuePair<Guid, TData>(kvp.Key.Guid, kvp.Value);
                    return true;
                }  
            }
            
            return false;
        }
    }
}
