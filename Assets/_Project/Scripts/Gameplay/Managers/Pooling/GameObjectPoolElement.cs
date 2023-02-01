using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Managers.Pooling
{
	public class GameObjectPoolElement : MonoBehaviour
	{
		[HideInInspector] public GameObjectPool ownerPoolScript;
		
		public void ReturnToOwnerPool(GameObject parentGameObject)
		{
			if (ownerPoolScript != null)
				ownerPoolScript.AddToQueue(parentGameObject);
			else
				throw new Exception("Can't add an object without an owner pool to the queue.");
		}
	}
}
