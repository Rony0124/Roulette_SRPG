using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using TSoft.Utils;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace TSoft.Data
{
   [Serializable]
    public struct RegistryId : IEquatable<RegistryId>, IComparable<RegistryId>
    {
        public static readonly RegistryId Null;
        
        [SerializeField]
        private byte[] _value;

        public Guid Value
        {
            get => _value is { Length: 16 } ? new Guid(_value) : Guid.Empty;
            set => _value = value.ToByteArray();
        } 

        public override string ToString() => Value.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RegistryId other) => Value.Equals(other.Value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object o) => _value.Equals((RegistryId)o);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => Value.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(RegistryId other) => Value.CompareTo(other.Value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator==(RegistryId obj1, RegistryId obj2) => obj1.Value.Equals(obj2.Value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator!=(RegistryId obj1, RegistryId obj2) => !obj1.Value.Equals(obj2.Value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator<(RegistryId obj1, RegistryId obj2) => obj1.CompareTo(obj2) < 0;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(RegistryId obj1, RegistryId obj2) => obj1.CompareTo(obj2) > 0;
    }
    
#if UNITY_EDITOR
    public class RegistryIdDrawer : OdinValueDrawer<RegistryId>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();

            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            GUIStyle style = new GUIStyle(GUI.skin.textField) { alignment = TextAnchor.MiddleRight };
            RegistryId registryId = this.ValueEntry.SmartValue;
            GUIHelper.PushLabelWidth(20);
            EditorGUI.SelectableLabel(rect.AlignLeft(rect.width), registryId.Value.ToString());
            GUIHelper.PopLabelWidth();
        }
    }
#endif
}
