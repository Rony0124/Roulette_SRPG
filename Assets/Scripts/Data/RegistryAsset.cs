using System;
using Sirenix.OdinInspector;
using TSoft.Utils;
using UnityEditor;
using UnityEngine;

namespace TSoft.Data
{
    public class RegistryAsset : ScriptableObject
    {
        [SerializeField]
        private RegistryId _id;
        
        public RegistryId Id => _id;
        
        [ScriptableObjectId, SerializeField, ReadOnly]
        private string _assetGuid;
        public string AssetGuid => _assetGuid;
        
        [SerializeField, ReadOnly]
        private string _lastGeneratedAssetGuid;
#if UNITY_EDITOR
        public void Awake()
        {
            UpdateIdentifier();
        }
        
        public void OnValidate()
        {
            UpdateIdentifier();
        }
        
        public void GenerateGUID()
        {
            if (Application.isPlaying)
                return;
            
            string assetPath = AssetDatabase.GetAssetPath(this);
            
            _id.Value = new Guid(AssetDatabase.AssetPathToGUID(assetPath));
            _lastGeneratedAssetGuid = _assetGuid;
            
            EditorUtility.SetDirty(this);
        }
        
        public void UpdateIdentifier()
        {
            if (_id == RegistryId.Null || _assetGuid != _lastGeneratedAssetGuid)
            {
                GenerateGUID();
            }
        }
#endif

    }
}
