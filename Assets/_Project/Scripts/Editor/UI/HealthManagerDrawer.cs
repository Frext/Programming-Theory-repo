using System;
using _Project.Scripts.Gameplay.Characters.Common;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor.UI
{
    [CustomEditor(typeof(HealthManager))]
    public class HealthManagerDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HealthManager myScript = (HealthManager)target;
            
            if(GUILayout.Button("Kill Me"))
            {
                myScript.TakeDamage(Int32.MaxValue);
            }
        }
    }
}