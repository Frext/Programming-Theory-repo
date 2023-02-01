using _Project.Scripts.UI;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor.UI
{
    [CustomPropertyDrawer(typeof(ShowIntObject.IntShowCondition))]
    public class IntShowConditionDrawer : PropertyDrawer
    {
        SerializedProperty conditionField;
        SerializedProperty valueField;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (conditionField == null || valueField == null)
            {
                conditionField = property.FindPropertyRelative("condition");
                valueField = property.FindPropertyRelative("value");
            }
            
            EditorGUI.BeginProperty(position, label, property);
            
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float widthSize = position.width / 3;

            Rect pos1 = new Rect(position.x, position.y, widthSize, position.height);
            Rect pos2 = new Rect(position.x + 175, position.y, widthSize / 2, position.height);

            EditorGUI.PropertyField(pos1, conditionField, GUIContent.none);
            EditorGUI.PropertyField(pos2, valueField, GUIContent.none);
            
            EditorGUI.EndProperty();
            
            EditorGUI.indentLevel = indent;
        }
    }
}