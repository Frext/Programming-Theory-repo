using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Data.Scriptable_Object_Templates
{
	[CreateAssetMenu(fileName = nameof(IntObject), menuName = "Scriptable Objects/" + nameof(IntObject), order = 0)]
	public class IntObject : ScriptableObject
	{
		public int value;
	}
}
