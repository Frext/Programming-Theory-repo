using System;
using _Project.Scripts.Gameplay.Characters.Player;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor.Characters
{
    [CustomEditor(typeof(PlayerHealth))]
    public class PlayerHealthDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PlayerHealth myScript = (PlayerHealth)target;
            
            if(GUILayout.Button("Kill Me"))
            {
                myScript.TakeDamage(Int32.MaxValue);
            }
        }
    }
}