using Hashira.Utils.PropertyTracker;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class PropertyTrackerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Debug.Log("님 ㅠㅠ");
        var targetType = target.GetType();
        var fields = targetType.GetFields(System.Reflection.BindingFlags.Instance |
                                        System.Reflection.BindingFlags.Public |
                                        System.Reflection.BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            if (field.GetCustomAttributes(typeof(PropertyTracker), false).Length > 0)
            {
                // SerializedProperty를 통해 필드 값 변경 감지
                var prop = serializedObject.FindProperty(field.Name);
                if (prop != null)
                {
                    EditorApplication.update += () =>
                    {
                        if (Application.isPlaying)
                        {
                            var currentValue = field.GetValue(target);
                            var targetObject = prop.serializedObject.targetObject;
                            var targetObjectClassType = targetObject.GetType();
                            var newField = targetObjectClassType.GetField(prop.propertyPath);
                            if (currentValue != null && !currentValue.Equals(newField.GetValue(target)))
                            {
                                var oldValue = newField.GetValue(target);
                                PropertyTracker.TrackChange(field.Name, oldValue, currentValue, target as MonoBehaviour);
                                newField.SetValue(target, currentValue);
                            }
                        }
                    };
                }
            }
        }
    }
    private void OnEnable()
    {
        Debug.Log("네");
    }
}
