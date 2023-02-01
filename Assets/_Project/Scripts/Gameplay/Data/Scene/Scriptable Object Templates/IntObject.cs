using UnityEngine;

namespace _Project.Scripts.Gameplay.Data.Scene.Scriptable_Object_Templates
{
	[CreateAssetMenu(fileName = nameof(IntObject), menuName = "Scriptable Objects/" + nameof(IntObject))]
	public class IntObject : ScriptableObject
	{
		public int initialValue;
		
		public int runtimeValue;

		private void OnEnable() => runtimeValue = initialValue;
	}
}
