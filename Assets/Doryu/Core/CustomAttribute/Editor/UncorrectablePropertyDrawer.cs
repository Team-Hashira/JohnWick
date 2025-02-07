using UnityEditor;
using UnityEngine;

namespace Doryu.CustomAttributes
{
    [CustomPropertyDrawer(typeof(UncorrectableAttribute))]
    public class UncorrectablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, true);
            EditorGUI.EndDisabledGroup();
        }
    }
}
