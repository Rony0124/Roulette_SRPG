using System;
using UnityEditor;
using UnityEngine;

namespace TSoft.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "RID", menuName = "DataID/Create Registry ID", order = 0)]
    public class DataRegistryIdSO : ScriptableObject, IComparable<DataRegistryIdSO>
    {
        [HideInInspector]
        [SerializeField]
        private byte[] guid;

        public Guid Guid => guid != null && guid.Length == 16 ? new Guid(guid) : Guid.Empty;
        
#if UNITY_EDITOR
        private void Awake()
        {
            if(guid == null || guid.Length == 0 || Guid == Guid.Empty)
                GenerateGUID();
        }
        
        void OnValidate()
        {
            if(guid == null || guid.Length == 0 || Guid == Guid.Empty)
                GenerateGUID();
        }
        
        public void GenerateGUID()
        {
            string assetPath = AssetDatabase.GetAssetPath(this);
            Guid assetGuid = new Guid(AssetDatabase.AssetPathToGUID(assetPath));
            guid = assetGuid.ToByteArray();

            EditorUtility.SetDirty(this);
        }
#endif
        
        public int CompareTo(DataRegistryIdSO other)
        {
            return Guid.CompareTo(other.Guid);
        }
    }
    
#if UNITY_EDITOR
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DataRegistryIdSO))]
    public class RegistryIdGuidEditor : Editor
    {
        private DataRegistryIdSO targetInfo;

        public void OnEnable() {
            if (targetInfo == null) {
                targetInfo = target as DataRegistryIdSO;
            }
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("GUID: " + targetInfo.Guid);
            if (targetInfo.Guid == Guid.Empty)
            {
                if (GUILayout.Button("Regenerate GUID")) {
                    targetInfo.GenerateGUID();
                }    
            }
            
            base.OnInspectorGUI();
        }
    }
#endif
}
