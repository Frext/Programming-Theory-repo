using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Managers.Pooling
{
	public class GameObjectPool : MonoBehaviour
	{
		[Header("Pool Properties")]
		[SerializeField] private GameObject poolObjectPrefab;
		[SerializeField] private int initialPoolSize;

		readonly Queue<GameObject> availablePoolObjects = new();
		

		[Header("Transform Properties")] 
		[SerializeField] private Transform parentTransform;
		
		
		void Awake()
		{
			GrowPoolBy(initialPoolSize);
		}

		private void GrowPoolBy(int objectCount)
		{
			for (int i = 0; i < objectCount; i++)
			{
				GameObject newPoolObject = Instantiate(poolObjectPrefab, parentTransform);

				DisableObject(newPoolObject);
				
				AssignOwnerPoolToChildren(newPoolObject);

				AddToQueue(newPoolObject);
			}
		}

		private void AssignOwnerPoolToChildren(GameObject newPoolObject)
		{
			// Assign this script for the pool element to return to the pool.
			newPoolObject.GetComponentInChildren<GameObjectPoolElement>().ownerPoolScript = this;
		}

		#region Methods Used By Other Scripts

		public void AddToQueue(GameObject returnedPoolObject)
		{
			DisableObject(returnedPoolObject);
			
			availablePoolObjects.Enqueue(returnedPoolObject);
		}

		private void DisableObject(GameObject poolObject)
		{
			poolObject.SetActive(false);
		}

		public List<GameObject> GetFromQueue(int objectCountToGet)
		{
			if (objectCountToGet <= 0)
				throw new Exception("Cannot get 0 or negative number objects from the pool");

			// Create new objects if there aren't enough in the pool.
			if (availablePoolObjects.Count < objectCountToGet)
			{
				GrowPoolBy(objectCountToGet - availablePoolObjects.Count);
			}
			

			List<GameObject> chosenPoolObjectsList = new();
			
			for (int i = 0; i < objectCountToGet; i++)
			{
				GameObject chosenPoolObject = availablePoolObjects.Dequeue();

				EnableObject(chosenPoolObject);

				chosenPoolObjectsList.Add(chosenPoolObject);
			}

			return chosenPoolObjectsList;
		}

		private void EnableObject(GameObject poolObject)
		{
			poolObject.SetActive(true);
		}

		#endregion
	}
}
