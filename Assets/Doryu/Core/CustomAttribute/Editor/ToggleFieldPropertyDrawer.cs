using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doryu.CustomAttributes
{
    [CustomPropertyDrawer(typeof(ToggleFieldAttribute))]
    public class ToggleFieldPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> _isExpandedDict = new Dictionary<string, bool>();

        private const float _ChiledInterval = 15f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float startXPos = position.x;
            // ToggleFieldAttribute를 가져오기
            ToggleFieldAttribute toggleField = (ToggleFieldAttribute)attribute;

            // 현재 객체에서 해당 속성을 찾음
            SerializedProperty toggleProperty = property.serializedObject.FindProperty(toggleField.PropertyName);

            bool isEnabled = false;
            if (toggleProperty != null)
            {
                if (toggleProperty.propertyType == SerializedPropertyType.Boolean)
                {
                    isEnabled = toggleProperty.boolValue == toggleField.BoolValue;
                }
                else if (toggleProperty.propertyType == SerializedPropertyType.Enum)
                {
                    isEnabled = toggleProperty.enumValueIndex == toggleField.EnumValue;
                }
            }

            // 토글 값에 따라 속성을 활성화/비활성화
            if (isEnabled)
            {
                EditorGUI.indentLevel++;

                // 연결 선
                float lineHight = EditorGUI.GetPropertyHeight(property, null, false) + EditorGUIUtility.standardVerticalSpacing;
                float lineWidth = 8;
                float lineXPos = position.x + _ChiledInterval * (EditorGUI.indentLevel - 1.5f);

                Rect verticalLine = new Rect(lineXPos, position.y - lineHight / 2 - 0.5f, 1.5f, lineHight + 1);
                EditorGUI.DrawRect(verticalLine, Color.gray);
                Rect horizontalLine = new Rect(lineXPos, position.y + lineHight / 2, lineWidth, 1);
                EditorGUI.DrawRect(horizontalLine, Color.gray);

                if (property.propertyType == SerializedPropertyType.Generic) // 구조체 처리
                {
                    string propertyPath = property.propertyPath;
                    bool isExpanded = _isExpandedDict.ContainsKey(propertyPath) ? _isExpandedDict[propertyPath] : false;

                    position.height = EditorGUI.GetPropertyHeight(property, null, false);
                    _isExpandedDict[propertyPath] = EditorGUI.Foldout(position, isExpanded, label, true);

                    if (isExpanded)
                    {
                        // 구조체의 필드 렌더링
                        EditorGUI.indentLevel++;
                        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                        SerializedProperty childProperty = property.Copy();
                        SerializedProperty endProperty = property.GetEndProperty();

                        childProperty.NextVisible(true); // 첫 번째 자식 필드로 이동
                        while (!SerializedProperty.EqualContents(childProperty, endProperty))
                        {
                            position.height = EditorGUI.GetPropertyHeight(childProperty, null, true);

                            Rect chiledVerticalLine = new Rect(lineXPos, position.y - lineHight / 2, 1.5f, position.height + EditorGUIUtility.standardVerticalSpacing);
                            EditorGUI.DrawRect(chiledVerticalLine, Color.gray);
                            Rect chiledHorizontalLine = new Rect(lineXPos, position.y + lineHight / 2, lineWidth + _ChiledInterval, 1);  
                            EditorGUI.DrawRect(chiledHorizontalLine, Color.gray);

                            EditorGUI.PropertyField(position, childProperty, true);
                            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                            childProperty.NextVisible(false);
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    // 일반 필드 처리
                    EditorGUI.PropertyField(position, property, label, true);
                }
                EditorGUI.indentLevel--;
            }
        }

        public float AddHorizontalInterval(ref Rect rect, float interval)
        {
            float prevLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth -= interval;
            rect.x += interval;
            rect.width -= interval;
            return prevLabelWidth;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ToggleFieldAttribute toggleField = (ToggleFieldAttribute)attribute;
            SerializedProperty toggleProperty = property.serializedObject.FindProperty(toggleField.PropertyName);

            bool isEnabled = false;
            if (toggleProperty != null)
            {
                if (toggleProperty.propertyType == SerializedPropertyType.Boolean)
                {
                    isEnabled = toggleProperty.boolValue == toggleField.BoolValue;
                }
                else if (toggleProperty.propertyType == SerializedPropertyType.Enum)
                {
                    isEnabled = toggleProperty.enumValueIndex == toggleField.EnumValue;
                }
            }

            if (isEnabled)
            {
                string propertyPath = property.propertyPath;
                bool isExpanded = _isExpandedDict.ContainsKey(propertyPath) ? _isExpandedDict[propertyPath] : false;

                if (property.propertyType == SerializedPropertyType.Generic && isExpanded)
                {
                    float height = EditorGUI.GetPropertyHeight(property, label, false);
                    if (property.hasVisibleChildren)
                    {
                        SerializedProperty tempProperty = property.Copy(); // 원래 위치 저장

                        if (property.NextVisible(true)) // 다음 속성이 존재하는지 확인
                        {
                            do
                            {
                                height += EditorGUI.GetPropertyHeight(property, null, true) + EditorGUIUtility.standardVerticalSpacing;
                            } while (property.NextVisible(false) && !SerializedProperty.EqualContents(property, tempProperty.GetEndProperty()));
                        }

                        property = tempProperty; // 원래 위치로 복원
                    }
                    height += EditorGUIUtility.standardVerticalSpacing;
                    return height;
                }
                else
                    return EditorGUIUtility.singleLineHeight;
            }
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
