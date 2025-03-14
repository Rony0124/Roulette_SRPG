using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace TSoft.Data.Registry
{
    public class Registry<T> : ScriptableObject
        where T : RegistryAsset
    {
        [SerializeField] private List<T> assets;
        
        public Dictionary<Guid, T> assetGuidLookup = new(); 
        public Dictionary<RegistryId, T> assetIdLookup = new();

        public int Count => assets.Count;
        
#if UNITY_EDITOR
        private void Awake()
        {
            SyncDictionaries();
        }
        
        void OnValidate()
        {
            SyncDictionaries();
        }
         
        private void SyncDictionaries()
        {
            assetGuidLookup.Clear();
            assetIdLookup.Clear();
            
            if (assets is not null && assets.Count > 0)
            {
                foreach (var data in assets)
                {
                    assetIdLookup.TryAdd(data.Id, data);
                    assetGuidLookup.TryAdd(data.Id.Value, data);
                }
            }
        }
#endif
        
        public T Get(RegistryId id)
        {
            return assetIdLookup.TryGetValue(id, out var asset) ? asset : null;
        }
        
        public T Get(Guid id)
        {
            return assetGuidLookup.TryGetValue(id, out var asset) ? asset : null;
        }

        public bool TryGetValue(RegistryId key, out T data)
        {
            return assetIdLookup.TryGetValue(key, out data);
        }
        
        public bool TryGetValue(Guid key, out T data)
        {
            return assetGuidLookup.TryGetValue(key, out data);
        }
    }
}
