using System;
using Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        private ConditionalHideAttribute GetCurrentHideAttribute()
        {
            // Get the attribute data for this property
            return (ConditionalHideAttribute)attribute;
        }

        private bool ShouldDrawProperty(SerializedProperty property)
        {
            if (GetHideAttributeResult(property))
                return true;

            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // You can use GUI.enabled to disable the property instead of hiding.
            
            if (ShouldDrawProperty(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ShouldDrawProperty(property))
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }

            // We want to undo the spacing added before and after the property for the property that is not being drawn
            return -EditorGUIUtility.standardVerticalSpacing;
        }

        private bool GetHideAttributeResult(SerializedProperty property)
        {
            SerializedProperty sourcePropertyValue =
                property.serializedObject.FindProperty(GetCurrentHideAttribute().ConditionalSourceField);

            if (sourcePropertyValue != null)
            {
                return sourcePropertyValue.boolValue;
            }

            throw new Exception(
                "Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " +
                GetCurrentHideAttribute().ConditionalSourceField);
        }
    }
}